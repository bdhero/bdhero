using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BDHero;
using BDHero.BDROM;
using BDHero.Plugin;
using BDHero.Startup;
using BDHero.Utils;
using BDHeroGUI.Forms;
using BDHeroGUI.Helpers;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Controls;
using DotNetUtils.Extensions;
using DotNetUtils.Forms;
using DotNetUtils.Net;
using Microsoft.Win32;
using OSUtils.DriveDetector;
using OSUtils.TaskbarUtils;
using UpdateLib;
using WindowsOSUtils.TaskbarUtils;

namespace BDHeroGUI
{
    [UsedImplicitly]
    public partial class FormMain : Form, IWndProcObservable
    {
        private const string PluginEnabledMenuItemName = "enabled";

        private readonly log4net.ILog _logger;
        private readonly IDirectoryLocator _directoryLocator;
        private readonly PluginLoader _pluginLoader;
        private readonly IController _controller;

        private readonly Updater _updater;
        private readonly IDriveDetector _driveDetector;
        private readonly UpdateHelper _updateHelper;

        private readonly ToolTip _progressBarToolTip;
        private readonly ITaskbarItem _taskbarItem;

        private bool _isRunning;

        private CancellationTokenSource _cancellationTokenSource;

        private ProgressProviderState _state = ProgressProviderState.Ready;
        private Stage _stage = Stage.None;

        public event WndProcEventHandler WndProcMessage;

        #region Properties

        private IList<INameProviderPlugin> EnabledNameProviderPlugins
        {
            get { return _controller.PluginsByType.OfType<INameProviderPlugin>().Where(plugin => plugin.Enabled).ToList(); }
        }

        #endregion

        #region Constructor and OnLoad

        public FormMain(log4net.ILog logger, IDirectoryLocator directoryLocator, PluginLoader pluginLoader, IController controller, Updater updater, IDriveDetector driveDetector)
        {
            InitializeComponent();

            Load += OnLoad;

            _logger = logger;
            _directoryLocator = directoryLocator;
            _pluginLoader = pluginLoader;
            _controller = controller;
            _updater = updater;
            _driveDetector = driveDetector;

            _progressBarToolTip = new ToolTip();
            _progressBarToolTip.SetToolTip(progressBar, null);

            _taskbarItem = new WindowsTaskbarItemFactory().GetInstance(Handle);

            progressBar.UseCustomColors = true;
            progressBar.GenerateText = percentComplete => string.Format("{0}: {1:0.00}%", _state, percentComplete);

            playlistListView.ItemSelectionChanged += PlaylistListViewOnItemSelectionChanged;
            playlistListView.ShowAllChanged += PlaylistListViewOnShowAllChanged;
            playlistListView.PlaylistReconfigured += PlaylistListViewOnPlaylistReconfigured;

            tracksPanel.PlaylistReconfigured += TracksPanelOnPlaylistReconfigured;

            mediaPanel.SelectedMediaChanged += MediaPanelOnSelectedMediaChanged;
            mediaPanel.Search = ShowMetadataSearchWindow;

            _updater.IsPortable = _directoryLocator.IsPortable;

            var updateObserver = new FormMainUpdateObserver(this, checkForUpdatesToolStripMenuItem, null);
            updateObserver.BeforeInstallUpdate += update => DisableUpdates();
            var currentVersion = AppUtils.AppVersion;
            _updateHelper = new UpdateHelper(_updater, currentVersion);
            _updateHelper.RegisterObserver(updateObserver);
            SystemEvents.SessionEnded += (sender, args) => DisableUpdates();

            FormClosing += OnFormClosing;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs args)
        {
            // Only prompt the user if they're currently scanning/muxing
            if (_state != ProgressProviderState.Running &&
                _state != ProgressProviderState.Paused)
            {
                return;
            }

            // Don't prompt the user if Windows is shutting down
            // or some other piece of code called Application.Exit()
            if (args.CloseReason == CloseReason.ApplicationExitCall ||
                args.CloseReason == CloseReason.WindowsShutDown)
            {
                return;
            }

            var title = string.Format("Close {0}?", AppUtils.AppName);
            var operation = _stage == Stage.Scan ? "scanning" : "conversion";
            var message = string.Format("Are you sure you want to close {0}?\n\nThis will abort the {1} process.",
                                        AppUtils.AppName, operation);

            var result = MessageBox.Show(this,
                                         message,
                                         title,
                                         MessageBoxButtons.OKCancel,
                                         MessageBoxIcon.Question,
                                         MessageBoxDefaultButton.Button2);

            if (result != DialogResult.OK)
            {
                args.Cancel = true;
            }
        }

        private void DisableUpdates()
        {
            _updateHelper.AllowInstallUpdate = false;
            _updater.CancelDownload();
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            Text += " v" + AppUtils.AppVersion;

            LogDirectoryPaths();
            LoadPlugins();
            LogPlugins();
            InitController();
            InitPluginMenu();
            InitUpdateCheck();

            EnableControls(true);
            splitContainerTop.Enabled = false;
            splitContainerMain.Enabled = false;

            this.EnableSelectAll();

            textBoxOutput.FileExtensions = new[]
                {
                    new FileExtension
                        {
                            Description = "Matroska video file",
                            Extensions = new[] {".mkv"}
                        }
                };

            InitDriveDetector();

            toolStripStatusLabelOffline.Visible = false;
            var monitor = new NetworkStatusMonitor(null, SetIsOnline);
        }

        private void SetIsOnline(bool isOnline)
        {
            toolStripStatusLabelOffline.Visible = !isOnline;
        }

        #endregion

        #region Win32 Window message handling

        /// <summary>
        /// This function receives all the windows messages for this window (form).
        /// We call the IDriveDetector from here so that is can pick up the messages about
        /// drives arrived and removed.
        /// </summary>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (WndProcMessage != null)
            {
                WndProcMessage(ref m);
            }
        }

        #endregion

        #region Initialization

        private void LogDirectoryPaths()
        {
            _logger.InfoFormat("IsPortable = {0}", _directoryLocator.IsPortable);
            _logger.InfoFormat("InstallDir = {0}", _directoryLocator.InstallDir);
            _logger.InfoFormat("AppConfigDir = {0}", _directoryLocator.AppConfigDir);
            _logger.InfoFormat("PluginConfigDir = {0}", _directoryLocator.PluginConfigDir);
            _logger.InfoFormat("RequiredPluginDir = {0}", _directoryLocator.RequiredPluginDir);
            _logger.InfoFormat("CustomPluginDir = {0}", _directoryLocator.CustomPluginDir);
            _logger.InfoFormat("LogDir = {0}", _directoryLocator.LogDir);
        }

        private void LoadPlugins()
        {
            _pluginLoader.LoadPlugins();
        }

        private void LogPlugins()
        {
            _pluginLoader.LogPlugins();
        }

        private void InitController()
        {
            _controller.ScanStarted += ControllerOnScanStarted;
            _controller.ScanSucceeded += ControllerOnScanSucceeded;
            _controller.ScanFailed += ControllerOnScanFailed;
            _controller.ScanCompleted += ControllerOnScanCompleted;

            _controller.ConvertStarted += ControllerOnConvertStarted;
            _controller.ConvertSucceeded += ControllerOnConvertSucceeded;
            _controller.ConvertFailed += ControllerOnConvertFailed;
            _controller.ConvertCompleted += ControllerOnConvertCompleted;

            _controller.PluginProgressUpdated += ControllerOnPluginProgressUpdated;
            _controller.UnhandledException += ControllerOnUnhandledException;
        }

        #endregion

        #region Plugins menu

        // TODO: Move logic to Controller
        private void InitPluginMenu()
        {
            Type prevPluginType = null;

            foreach (var plugin in _controller.PluginsByType)
            {
                IPlugin pluginCopy = plugin;

                var ifaces = plugin.GetType().GetInterfaces().Except(new[] {typeof(IPlugin)});
                var curPluginType = ifaces.FirstOrDefault(type => type.GetInterfaces().Contains(typeof(IPlugin)));
                if (prevPluginType != null && prevPluginType != curPluginType)
                {
                    pluginsToolStripMenuItem.DropDownItems.Add("-");
                }

                var iconImage16 = plugin.Icon != null ? new Icon(plugin.Icon, new Size(16, 16)).ToBitmap() : null;
                var pluginItem = new ToolStripMenuItem(plugin.Name) { Image = iconImage16, Tag = plugin };

                var enabledItem = new ToolStripMenuItem("Enabled") { CheckOnClick = true, Name = PluginEnabledMenuItemName };
                enabledItem.Click += delegate(object sender, EventArgs args)
                    {
                        if (pluginCopy is IDiscReaderPlugin)
                        {
                            _controller.PluginsByType.OfType<IDiscReaderPlugin>()
                                       .ForEach(readerPlugin => readerPlugin.Enabled = readerPlugin == pluginCopy);
                        }
                        else
                        {
                            pluginCopy.Enabled = enabledItem.Checked;
                        }
                        AutoCheckPluginMenu();
                    };

                pluginItem.DropDownItems.Add(enabledItem);

                if (plugin.EditPreferences != null)
                {
                    pluginItem.DropDownItems.Add(new ToolStripMenuItem("Preferences...", null,
                                                                        (sender, args) =>
                                                                        EditPreferences(pluginCopy)));
                }

                pluginItem.DropDownItems.Add("-");

                pluginItem.DropDownItems.Add(
                    new ToolStripMenuItem(string.Format("Run order: {0}", plugin.RunOrder)) { Enabled = false });
                pluginItem.DropDownItems.Add(
                    new ToolStripMenuItem(string.Format("Version {0}", plugin.AssemblyInfo.Version)) { Enabled = false });
                pluginItem.DropDownItems.Add(
                    new ToolStripMenuItem(string.Format("Built on {0}", plugin.AssemblyInfo.BuildDate)) { Enabled = false });

                pluginsToolStripMenuItem.DropDownItems.Add(pluginItem);

                prevPluginType = curPluginType;
            }

            AutoCheckPluginMenu();
        }

        private void EditPreferences(IPlugin plugin)
        {
            if (DialogResult.OK == plugin.EditPreferences(this))
            {
                if (plugin is INameProviderPlugin)
                {
                    RenameSync();
                }
            }
        }

        // TODO: Move logic to Controller
        private void AutoCheckPluginMenu()
        {
            var allItems = pluginsToolStripMenuItem.DropDownItems.OfType<ToolStripMenuItem>().ToArray();

            var allDiscReaderPlugins = allItems.Select(item => item.Tag as IDiscReaderPlugin).Where(plugin => plugin != null).ToArray();
            var enabledDiscReaderPlugins = allDiscReaderPlugins.Where(plugin => plugin.Enabled).ToArray();

            if (enabledDiscReaderPlugins.Any())
            {
                // Only allow one Disc Reader plugin to be enabled at a time
                enabledDiscReaderPlugins.Skip(1).ForEach(plugin => plugin.Enabled = false);
            }
            else
            {
                // Require one Disc Reader to always be enabled
                allDiscReaderPlugins.First().Enabled = true;
            }

            foreach (var item in allItems.Where(item => item.Tag is IPlugin))
            {
                var plugin = item.Tag as IPlugin;
                var enabledItem = item.DropDownItems[PluginEnabledMenuItemName] as ToolStripMenuItem;

                if (plugin == null || enabledItem == null)
                    continue;

                enabledItem.Checked = plugin.Enabled;

                if (plugin is IDiscReaderPlugin)
                {
                    // Exactly one Disc Reader plugin must be enabled at any time; no more, no less.
                    // Don't allow users to disable a Disc Reader plugin -- only allow them to enable other ones
                    // (which will disable the previously enabled one).
                    enabledItem.Enabled = !plugin.Enabled;
                }
            }

            linkLabelNameProviderPreferences.Enabled = EnabledNameProviderPlugins.Any();
        }

        #endregion

        #region Updates

        private void InitUpdateCheck()
        {
            Disposed += (sender, args) => InstallUpdateIfAvailable(true);
            _updateHelper.CheckForUpdates();
        }

        private void InstallUpdateIfAvailable(bool silent)
        {
            _updateHelper.InstallUpdateIfAvailable(silent);
        }

        #endregion

        #region Disk Detection

        private void InitDriveDetector()
        {
            openDiscToolStripMenuItem.Initialize(this, _driveDetector);
            openDiscToolStripMenuItem.DiscSelected += driveInfo => Scan(driveInfo.Name);
        }

        #endregion

        #region Cancellation

        private CancellationTokenSource CreateCancellationTokenSource()
        {
            return _cancellationTokenSource = new CancellationTokenSource();
        }

        private bool IsCancellationRequested
        {
            get { return _cancellationTokenSource != null && _cancellationTokenSource.IsCancellationRequested; }
        }

        private void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        #endregion

        #region Metadata search

        private void ShowMetadataSearchWindow()
        {
            var job = _controller.Job;

            if (job == null) return;

            var form = new FormMetadataSearch(job.SearchQuery);

            if (DialogResult.OK != form.ShowDialog(this)) return;

            job.SearchQuery = form.SearchQuery;

            _controller
                .CreateMetadataTask(CreateCancellationTokenSource().Token, GetMetadataStart, GetMetadataFail, GetMetadataSucceed)
                .Start()
                ;
        }

        private void GetMetadataStart()
        {
            buttonScan.Text = "Searching...";
            textBoxStatus.Text = "Searching for metadata...";
            EnableControls(false);
            _taskbarItem.SetProgress(0).Indeterminate();
        }

        private void GetMetadataSucceed()
        {
            // TODO: Centralize button text
            buttonScan.Text = "Scan";
            textBoxOutput.Text = _controller.Job.OutputPath;
            AppendStatus("Metadata search completed successfully!");
            _taskbarItem.NoProgress();
            RefreshUI();
            EnableControls(true);
        }

        private void GetMetadataFail(ExceptionEventArgs args)
        {
            // TODO: Centralize button text
            buttonScan.Text = "Scan";

            if (IsCancellationRequested)
            {
                AppendStatus("Metadata search canceled!");
                _taskbarItem.NoProgress();
            }
            else
            {
                AppendStatus("Metadata search failed!");
                _taskbarItem.Error();
            }

            if (args.Exception != null)
            {
                DetailForm.ShowExceptionDetail(this, "Error: Metadata Search Failed", args.Exception);
            }

            EnableControls(true);
        }

        #endregion

        #region File renamer

        private void RenameSync()
        {
            if (_controller.Job == null)
                return;
            _controller.RenameSync(null);
            textBoxOutput.Text = _controller.Job.OutputPath;
        }

        #endregion

        #region UI

        private void RefreshUI()
        {
            RefreshPlaylists();
            playlistListView.SelectFirstPlaylist();
            mediaPanel.Job = _controller.Job;
        }

        private void RefreshPlaylists()
        {
            if (_controller.Job != null)
                playlistListView.Playlists = _controller.Job.Disc.Playlists;
        }

        private void EnableControls(bool enabled)
        {
            var isPlaylistSelected = playlistListView.SelectedPlaylist != null;
            var hasJob = _controller.Job != null;

            openBDROMFolderToolStripMenuItem.Enabled = enabled;
            openDiscToolStripMenuItem.Enabled = enabled;
            searchForMetadataToolStripMenuItem.Enabled = enabled && hasJob;

            discInfoToolStripMenuItem.Enabled = enabled && hasJob;
            filterPlaylistsToolStripMenuItem.Enabled = enabled;
            showAllPlaylistsToolStripMenuItem.Enabled = enabled;
            filterTracksToolStripMenuItem.Enabled = enabled;
            showAllTracksToolStripMenuItem.Enabled = enabled;

//            optionsToolStripMenuItem.Enabled = enabled;

            textBoxInput.Enabled = enabled;
            buttonScan.Enabled = enabled;
            buttonCancelScan.Enabled = !enabled;
            rescanToolStripMenuItem.Enabled = enabled;

            textBoxOutput.Enabled = enabled;
            buttonConvert.Enabled = enabled && isPlaylistSelected;
            buttonCancelConvert.Enabled = !enabled && isPlaylistSelected;
            linkLabelNameProviderPreferences.Enabled = enabled;

            splitContainerTop.Enabled = enabled && hasJob;
            splitContainerMain.Enabled = enabled && hasJob;

            _isRunning = !enabled;
        }

        private void AppendStatus(string statusLine = null)
        {
            textBoxStatus.Text = (statusLine ?? string.Empty);
            _logger.Debug(statusLine);
        }

        #endregion

        #region Scan & Convert stages

        /// <summary>
        /// Asynchronously scans the selected BD-ROM folder.
        /// </summary>
        /// <param name="path">Optional path to the root BD-ROM folder.  If specified, the "Source BD-ROM" textbox will be populated with this path.</param>
        private void Scan(string path = null)
        {
            if (path != null)
                textBoxInput.Text = path;

            _controller.SetEventScheduler();

            // TODO: Let File Namer plugin handle this
            var outputDirectory = FileUtils.ContainsFileName(textBoxOutput.Text)
                                      ? Path.GetDirectoryName(textBoxOutput.Text)
                                      : textBoxOutput.Text;
            _controller
                .CreateScanTask(CreateCancellationTokenSource().Token, textBoxInput.Text, outputDirectory)
                .Start();
        }

        /// <summary>
        /// Asynchronously converts the BD-ROM to an MKV file according to the user's settings and selections.
        /// </summary>
        private void Convert()
        {
            var selectedPlaylist = playlistListView.SelectedPlaylist;
            if (selectedPlaylist == null)
            {
                MessageBox.Show(this, "Please select a playlist to mux", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                return;
            }

            _controller.Job.SelectedPlaylistIndex = _controller.Job.Disc.Playlists.IndexOf(selectedPlaylist);

            _controller.SetEventScheduler();

            _controller
                .CreateConvertTask(CreateCancellationTokenSource().Token, textBoxOutput.Text)
                .Start();
        }

        #endregion

        #region Scan events

        private void ControllerOnScanStarted()
        {
            _stage = Stage.Scan;
            buttonScan.Text = "Scanning...";
            textBoxStatus.Text = "Scan started...";
            EnableControls(false);
            _taskbarItem.SetProgress(0).Indeterminate();
        }

        private void ControllerOnScanSucceeded()
        {
            textBoxOutput.Text = _controller.Job.OutputPath;
            AppendStatus("Scan succeeded!");
            _taskbarItem.NoProgress();
            RefreshUI();
        }

        private void ControllerOnScanFailed(ExceptionEventArgs args)
        {
            if (IsCancellationRequested)
            {
                AppendStatus("Scan canceled!");
                _taskbarItem.NoProgress();
            }
            else
            {
                AppendStatus("Scan failed!");
                _taskbarItem.Error();
            }
            if (args.Exception != null)
            {
                DetailForm.ShowExceptionDetail(this, "Error: Scan Failed", args.Exception);
            }
        }

        private void ControllerOnScanCompleted()
        {
            _stage = Stage.None;
            buttonScan.Text = "Scan";
            EnableControls(true);
        }

        #endregion

        #region Convert events

        private void ControllerOnConvertStarted()
        {
            _stage = Stage.Convert;
            buttonConvert.Text = "Converting...";
            AppendStatus("Convert started...");
            EnableControls(false);
            _taskbarItem.SetProgress(0).Indeterminate();
        }

        private void ControllerOnConvertSucceeded()
        {
            AppendStatus("Convert succeeded!");
            _taskbarItem.NoProgress();
        }

        private void ControllerOnConvertFailed(ExceptionEventArgs args)
        {
            if (IsCancellationRequested)
            {
                AppendStatus("Convert canceled!");
                _taskbarItem.NoProgress();
            }
            else
            {
                AppendStatus("Convert failed!");
                _taskbarItem.Error();
            }
            if (args.Exception != null)
            {
                DetailForm.ShowExceptionDetail(this, "Error: Convert Failed", args.Exception);
            }
        }

        private void ControllerOnConvertCompleted()
        {
            _stage = Stage.None;
            buttonConvert.Text = "Convert";
            EnableControls(true);
        }

        #endregion

        #region Progress events

        private void ControllerOnPluginProgressUpdated(IPlugin plugin, ProgressProvider progressProvider)
        {
            if (!_isRunning)
                return;

            _state = progressProvider.State;

            var percentCompleteStr = (progressProvider.PercentComplete/100.0).ToString("P");
            var line = string.Format("{0} is {1} - {2} complete - {3} - {4} elapsed, {5} remaining",
                                     plugin.Name, progressProvider.State, percentCompleteStr,
                                     progressProvider.Status,
                                     progressProvider.RunTime.ToStringShort(),
                                     progressProvider.TimeRemaining.ToStringShort());
            AppendStatus(line);

            progressBar.ValuePercent = progressProvider.PercentComplete;
            _progressBarToolTip.SetToolTip(progressBar, string.Format("{0}: {1}", progressProvider.State, percentCompleteStr));
            _taskbarItem.Progress = progressProvider.PercentComplete;

            switch (progressProvider.State)
            {
                case ProgressProviderState.Error:
                    progressBar.SetError();
                    _taskbarItem.Error();
                    break;
                case ProgressProviderState.Paused:
                    progressBar.SetPaused();
                    _taskbarItem.Pause();
                    break;
                case ProgressProviderState.Canceled:
                    progressBar.SetMuted();
                    _taskbarItem.NoProgress();
                    break;
                default:
                    progressBar.SetSuccess();
                    _taskbarItem.Normal();
                    break;
            }
        }

        #endregion

        #region Exception handling

        private void ControllerOnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            var caption = string.Format("{0} Error", AppUtils.AppName);
            DetailForm.ShowExceptionDetail(this, caption, args.ExceptionObject as Exception);
        }

        #endregion

        #region UI events - ToolStrip MenuItems

        private void openBDROMFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxInput.Browse();
        }

        private void rescanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scan();
        }

        private void searchForMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMetadataSearchWindow();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void discInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.Job != null)
            {
                new FormDiscInfo(_controller.Job.Disc).ShowDialog(this);
            }
        }

        private void filterPlaylistsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playlistListView.ShowFilterWindow();
        }

        private void showAllPlaylistsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            playlistListView.ShowAll = !playlistListView.ShowAll;
        }

        private void filterTracksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tracksPanel.ShowFilterWindow();
        }

        private void showAllTracksToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            tracksPanel.ShowAll = showAllTracksToolStripMenuItem.Checked;
        }

        private void homepageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileUtils.OpenUrl(AppConstants.ProjectHomepage);
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileUtils.OpenUrl(AppConstants.DocumentationUrl);
        }

        private void submitABugReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileUtils.OpenUrl(AppConstants.BugReportUrl);
        }

        private void suggestAFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileUtils.OpenUrl(AppConstants.SuggestFeatureUrl);
        }

        private void showLogFileInWindowsExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileUtils.OpenFolder(_directoryLocator.LogDir);
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _updateHelper.CheckForUpdates();
        }

        private void aboutBDHeroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog(this);
        }

        #endregion

        #region UI events - button clicks

        private void buttonScan_Click(object sender, EventArgs e)
        {
            Scan();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            Convert();
        }

        private void buttonCancelConvert_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        #endregion

        #region UI events - LinkLabel clicks

        private void linkLabelNameProviderPreferences_Click(object sender, EventArgs e)
        {
            var plugins = EnabledNameProviderPlugins;

            if (!plugins.Any())
                return;

            if (plugins.Count == 1)
            {
                EditPreferences(plugins.First());
            }
            else
            {
                var menu = new ContextMenuStrip();
                foreach (var plugin in plugins)
                {
                    menu.Items.Add(GetNameProviderPluginPreferenceMenuItem(plugin));
                }
                menu.Show(linkLabelNameProviderPreferences, 0, linkLabelNameProviderPreferences.Height);
            }
        }

        private ToolStripMenuItem GetNameProviderPluginPreferenceMenuItem(INameProviderPlugin plugin)
        {
            var image = plugin.Icon != null ? plugin.Icon.ToBitmap() : null;
            var item = new ToolStripMenuItem(plugin.Name, image, (sender, args) => EditPreferences(plugin));
            return item;
        }

        #endregion

        #region UI events - selection change

        private void textBoxInput_SelectedPathChanged(object sender, EventArgs e)
        {
            Scan();
        }

        private void MediaPanelOnSelectedMediaChanged(object sender, EventArgs eventArgs)
        {
            RenameSync();
        }

        private void PlaylistListViewOnItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs args)
        {
            var playlist = playlistListView.SelectedPlaylist;

            _controller.Job.SelectedPlaylist = playlist;
            buttonConvert.Enabled = playlist != null;
            tracksPanel.SetPlaylist(playlist, _controller.Job.Disc.Languages.ToArray());
            chaptersPanel.Playlist = playlist;

            RenameSync();
        }

        private void PlaylistListViewOnShowAllChanged(object sender, EventArgs eventArgs)
        {
            showAllPlaylistsToolStripMenuItem.Checked = playlistListView.ShowAll;
        }

        private void PlaylistListViewOnPlaylistReconfigured(Playlist playlist)
        {
            RenameSync();
        }

        private void TracksPanelOnPlaylistReconfigured(Playlist playlist)
        {
            playlistListView.ReconfigurePlaylist(playlist);
            RenameSync();
        }

        #endregion

        #region Drag and Drop

        private static string GetFirstBDROMDirectory(DragEventArgs e)
        {
            return DragUtils.GetPaths(e).Select(BDFileUtils.GetBDROMDirectory).FirstOrDefault(s => s != null);
        }

        private bool AcceptBDROMDrop(DragEventArgs e)
        {
            return _state != ProgressProviderState.Running
                && _state != ProgressProviderState.Paused
                && GetFirstBDROMDirectory(e) != null
                ;
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (AcceptBDROMDrop(e))
            {
                e.Effect = DragDropEffects.All;
                textBoxInput.Highlight();
            }
            else
            {
                e.Effect = DragDropEffects.None;
                textBoxInput.UnHighlight();
            }
        }

        private void FormMain_DragLeave(object sender, EventArgs e)
        {
            textBoxInput.UnHighlight();
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            if (!AcceptBDROMDrop(e)) return;
            Scan(GetFirstBDROMDirectory(e));
        }

        #endregion
    }

    internal enum Stage
    {
        None,
        Scan,
        Convert
    }
}
