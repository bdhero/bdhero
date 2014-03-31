// Copyright 2012-2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

#define DISABLE_UPDATER

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BDHero;
using BDHero.BDROM;
using BDHero.ErrorReporting;
using BDHero.Exceptions;
using BDHero.JobQueue;
using BDHero.Plugin;
using BDHero.Prefs;
using BDHero.Startup;
using BDHero.SyntaxHighlighting;
using BDHero.Utils;
using BDHeroGUI.Components;
using BDHeroGUI.Dialogs;
using BDHeroGUI.Forms;
using BDHeroGUI.Helpers;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Concurrency;
using DotNetUtils.Extensions;
using DotNetUtils.FS;
using log4net;
using Microsoft.WindowsAPICodePack.Taskbar;
using OSUtils.DriveDetector;
using OSUtils.Net;
using OSUtils.TaskbarUtils;
using OSUtils.Window;
using TextEditor;
using UILib.Extensions;
using UILib.WinForms;

namespace BDHeroGUI
{
    [UsedImplicitly]
    public partial class FormMain : Form, IErrorReportResultVisitor
    {
        private const string PluginEnabledMenuItemName = "enabled";

        private readonly ILog _logger;
        private readonly IDirectoryLocator _directoryLocator;
        private readonly IPreferenceManager _preferenceManager;
        private readonly PluginLoader _pluginLoader;
        private readonly IPluginRepository _pluginRepository;
        private readonly IController _controller;
        private readonly IDriveDetector _driveDetector;
        private readonly ITaskbarItem _taskbarItem;
        private readonly IWindowMenuFactory _windowMenuFactory;
        private readonly INetworkStatusMonitor _networkStatusMonitor;

#if ENABLE_UPDATER
        private readonly Updater _updater;
        private readonly UpdateHelper _updateHelper;
        private bool _hasCheckedForUpdateOnStartup;
#endif

        private readonly ToolTip _progressBarToolTip;

        private bool _isRunning;
        private CancellationTokenSource _cancellationTokenSource;

        private ProgressProviderState _state = ProgressProviderState.Ready;
        private Stage _stage = Stage.None;

        public string[] Args = new string[0];

        #region Properties

        private IList<INameProviderPlugin> EnabledNameProviderPlugins
        {
            get { return _controller.PluginsByType.OfType<INameProviderPlugin>().Where(plugin => plugin.Enabled).ToList(); }
        }

        #endregion

        #region Constructor and OnLoad

        public FormMain(ILog logger, IDirectoryLocator directoryLocator, IPreferenceManager preferenceManager,
                        PluginLoader pluginLoader, IPluginRepository pluginRepository, IController controller,
                        IDriveDetector driveDetector, ITaskbarItemFactory taskbarItemFactory, IWindowMenuFactory windowMenuFactory,
                        INetworkStatusMonitor networkStatusMonitor
#if ENABLE_UPDATER
                        , Updater updater
#endif
            )
        {
            InitializeComponent();

            Load += OnLoad;

            _logger = logger;
            _directoryLocator = directoryLocator;
            _preferenceManager = preferenceManager;
            _pluginLoader = pluginLoader;
            _pluginRepository = pluginRepository;
            _controller = controller;
            _driveDetector = driveDetector;
            _taskbarItem = taskbarItemFactory.GetInstance(Handle);
            _windowMenuFactory = windowMenuFactory;
            _networkStatusMonitor = networkStatusMonitor;

            
#if ENABLE_UPDATER
            _updater = updater;
            _updater.IsPortable = _directoryLocator.IsPortable;
            _updateHelper = new UpdateHelper(_updater, AppUtils.AppVersion) { AllowDownload = false };
#else
            updateToolStripMenuItem.Visible = false;
            checkForUpdatesToolStripMenuItem.Enabled = false;
#endif

            _progressBarToolTip = new ToolTip();
            _progressBarToolTip.SetToolTip(progressBar, null);

            progressBar.UseCustomColors = true;
            progressBar.GenerateText = percentComplete => string.Format("{0}: {1:0.00}%", _state, percentComplete);

            playlistListView.ItemSelectionChanged += PlaylistListViewOnItemSelectionChanged;
            playlistListView.ShowAllChanged += PlaylistListViewOnShowAllChanged;
            playlistListView.PlaylistReconfigured += PlaylistListViewOnPlaylistReconfigured;

            tracksPanel.PlaylistReconfigured += TracksPanelOnPlaylistReconfigured;

            mediaPanel.SelectedMediaChanged += MediaPanelOnSelectedMediaChanged;
            mediaPanel.Search = ShowMetadataSearchWindow;
            
#if ENABLE_UPDATER
            var updateObserver = new FormMainUpdateObserver(this,
                                                            checkForUpdatesToolStripMenuItem,
                                                            updateToolStripMenuItem,
                                                            downloadUpdateToolStripMenuItem);
            updateObserver.BeforeInstallUpdate += update => DisableUpdates();
            SystemEvents.SessionEnded += (sender, args) => DisableUpdates();
            _updateHelper.RegisterObserver(updateObserver);
#endif

            FormClosing += OnFormClosing;

            var recentFiles = _preferenceManager.Preferences.RecentFiles;
            if (recentFiles.RememberRecentFiles && recentFiles.RecentBDROMPaths.Any())
            {
                textBoxInput.Text = recentFiles.RecentBDROMPaths.First();
            }

            InitSystemMenu();

            InitTaskBarId();
        }

        private void InitTaskBarId()
        {
            if (TaskbarManager.IsPlatformSupported)
            {
                var manager = TaskbarManager.Instance;
                manager.ApplicationId = string.Format("{0}.{1}.{2}.{3:s}",
                                                      AppUtils.ProductName, AppUtils.AppName,
                                                      AppUtils.AppVersion, AppUtils.BuildDate);
            }
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
        
#if ENABLE_UPDATER
        private void DisableUpdates()
        {
            _updateHelper.AllowInstallUpdate = false;
            _updater.CancelDownload();
        }
#endif

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

            textBoxOutput.FileTypes = new[]
                {
                    new FileType
                        {
                            Description = "Matroska video file",
                            Extensions = new[] {".mkv"}
                        }
                };

            InitDriveDetector();

            toolStripStatusLabelOffline.Visible = false;

            // TODO: Add setting to enable/disable
            InitNetworkStatusMonitor();

            InitAboutBox();
            InitTextEditors();

            ScanOnStartup();
        }

        private void InitNetworkStatusMonitor()
        {
            _networkStatusMonitor.NetworkStatusChanged += NetworkStatusMonitorOnNetworkStatusChanged;
            _networkStatusMonitor.TestConnectionAsync();
        }

        private void NetworkStatusMonitorOnNetworkStatusChanged(bool isConnectedToInternet)
        {
            Invoke(new Action(() => SetIsOnline(isConnectedToInternet)));
        }

        private void SetIsOnline(bool isOnline)
        {
            toolStripStatusLabelOffline.Visible = !isOnline;
            
#if ENABLE_UPDATER
            if (!isOnline || _hasCheckedForUpdateOnStartup)
                return;

            _updateHelper.Click();
            _hasCheckedForUpdateOnStartup = true;
#endif
        }

        /// <summary>
        ///     The <see cref="AboutBox"/> takes several seconds to initialize the first time
        ///     it is constructed, so preemptively instantiate it in a
        ///     background thread to speed up loading when the user actually opens it.
        /// </summary>
        private void InitAboutBox()
        {
            Task.Factory.StartNew(InitAboutBoxImpl);
        }

        private void InitAboutBoxImpl()
        {
            using (var form = new AboutBox(_pluginRepository))
            {
            }
        }

        private void ShowAboutBox()
        {
            using (var form = new AboutBox(_pluginRepository))
            {
                form.ShowDialog(this);
            }
        }

        private void InitTextEditors()
        {
            var editor = TextEditorFactory.CreateMultiLineTextEditor();
            editor.LoadSyntaxDefinitions(new BDHeroT4SyntaxModeProvider());
        }

        private void ScanOnStartup()
        {
            var path = Args.FirstOrDefault(arg => File.Exists(arg) || Directory.Exists(arg));
            if (path == null)
                return;
            Scan(path);
        }

        #endregion

        #region Error Reporting

        private void ShowErrorDialog(string title, Exception exception, bool isReportable = true)
        {
            var report = new ErrorReport(exception, _pluginRepository, _directoryLocator);

            IErrorDialog dialog;

            if (Windows7ErrorDialog.IsPlatformSupported)
            {
                dialog = new Windows7ErrorDialog(_networkStatusMonitor, report);
            }
            else
            {
                dialog = new GenericErrorDialog(report);
            }

            dialog.Title = title;

            dialog.AddResultVisitor(this);

            if (isReportable && !IsID10TError(exception))
            {
                dialog.ShowReportable(this);
            }
            else
            {
                dialog.ShowNonReportable(this);
            }
        }

        private void ShowNonReportableErrorDialog(Exception exception)
        {
            const string title = "Unable to submit error report";
            ShowErrorDialog(title, exception, false);
        }

        public void Visit(ErrorReportResultCreated result)
        {
            var panel = new ToolStripControlBuilder()
                .AddLabel("Submitted")
                .AddHyperlink(result)
                .Build();
            statusStrip1.Items.Add(panel);
        }

        public void Visit(ErrorReportResultUpdated result)
        {
            var panel = new ToolStripControlBuilder()
                .AddLabel("Updated")
                .AddHyperlink(result)
                .Build();
            statusStrip1.Items.Add(panel);
        }

        public void Visit(ErrorReportResultFailed result)
        {
            var panel = new ToolStripControlBuilder()
                .AddImage(Properties.Resources.error_circle)
                .AddLabel("Unable to submit error report")
                .AddLink("(details)", (sender, args) => ShowNonReportableErrorDialog(result.Exception))
                .Build();
            statusStrip1.Items.Add(panel);
        }

        /// <summary>
        ///     Determines if the given exception is likely due to user error (ID10T).
        /// </summary>
        /// <param name="e">
        ///     Exception that was thrown elsewhere in the application.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the given exception is likely due to user error; otherwise <c>false</c>.
        /// </returns>
        private static bool IsID10TError(Exception e)
        {
            return false;
            return (e is ID10TException ||
                    e is DirectoryNotFoundException ||
                    e is DriveNotFoundException ||
                    e is FileNotFoundException ||
                    e is PathTooLongException ||
                    e is WebException);
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
            _controller.BeforeScanStart += ControllerOnBeforeScanStart;
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
            _controller.PluginsByType.Aggregate<IPlugin, Type>(null, InitPlugin);
            AutoCheckPluginMenu();
        }

        private Type InitPlugin([CanBeNull] Type prevPluginType, [NotNull] IPlugin plugin)
        {
            var curPluginInterfaces = plugin.GetType().GetInterfaces().Except(new[] { typeof(IPlugin) });
            var curPluginType = curPluginInterfaces.FirstOrDefault(type => type.GetInterfaces().Contains(typeof(IPlugin)));

            if (prevPluginType != null && prevPluginType != curPluginType)
            {
                pluginsToolStripMenuItem.DropDownItems.Add("-");
            }

            var iconImage16 = plugin.Icon != null ? new Icon(plugin.Icon, new Size(16, 16)).ToBitmap() : null;
            var pluginItem = new ToolStripMenuItem(plugin.Name) { Image = iconImage16, Tag = plugin };

            var enabledItem = new ToolStripMenuItem("Enabled") { CheckOnClick = true, Name = PluginEnabledMenuItemName };
            enabledItem.Click += delegate
                {
                    if (plugin is IDiscReaderPlugin)
                    {
                        _controller.PluginsByType.OfType<IDiscReaderPlugin>()
                                   .ForEach(readerPlugin => readerPlugin.Enabled = readerPlugin == plugin);
                    }
                    else
                    {
                        plugin.Enabled = enabledItem.Checked;
                        _preferenceManager.UpdatePreferences(prefs =>
                                                             prefs.Plugins.SetPluginEnabled(plugin.AssemblyInfo.Guid,
                                                                                            plugin.Enabled));
                    }
                    AutoCheckPluginMenu();
                };

            pluginItem.DropDownItems.Add(enabledItem);

            if (plugin.EditPreferences != null)
            {
                pluginItem.DropDownItems.Add(new ToolStripMenuItem("Preferences...", null,
                                                                   (sender, args) =>
                                                                   EditPreferences(plugin)));
            }

            pluginItem.DropDownItems.Add("-");

            pluginItem.DropDownItems.Add(
                new ToolStripMenuItem(string.Format("Run order: {0}", plugin.RunOrder)) { Enabled = false });
            pluginItem.DropDownItems.Add(
                new ToolStripMenuItem(string.Format("Version {0}", plugin.AssemblyInfo.Version)) { Enabled = false });
            pluginItem.DropDownItems.Add(
                new ToolStripMenuItem(string.Format("Built on {0}", plugin.AssemblyInfo.BuildDate)) { Enabled = false });

            pluginsToolStripMenuItem.DropDownItems.Add(pluginItem);

            return curPluginType;
        }

        private void EditPreferences(IPlugin plugin)
        {
            if (DialogResult.OK == plugin.EditPreferences(this))
            {
                if (plugin is INameProviderPlugin)
                {
                    RenameAsync();
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
        }

        private void InstallUpdateIfAvailable(bool silent)
        {
            // TODO: Re-enable when automatic updating is implemented
//            _updateHelper.InstallUpdateIfAvailable(silent);
        }

        #endregion

        #region Disk Detection

        private void InitDriveDetector()
        {
            openDiscToolStripMenuItem.Initialize(this, _driveDetector);
            openDiscToolStripMenuItem.DiscSelected += driveInfo => Scan(driveInfo.Name);
        }

        #endregion

        #region System menu

        private void InitSystemMenu()
        {
            new StandardWindowMenuBuilder(this, _windowMenuFactory)
                .Resize()
                .AlwaysOnTop();
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

            DialogResult result;
            SearchQuery searchQuery;

            using (var form = new FormMetadataSearch(job.SearchQuery))
            {
                result = form.ShowDialog(this);
                searchQuery = form.SearchQuery;
            }

            if (DialogResult.OK != result)
            {
                return;
            }

            job.SearchQuery = searchQuery;

            _controller
                .CreateMetadataTask(CreateCancellationTokenSource().Token, GetMetadataStart, GetMetadataFail, GetMetadataSucceed)
                .Start()
                ;
        }

        private void GetMetadataStart(IPromise<bool> promise)
        {
            buttonScan.Text = "Searching...";
            textBoxStatus.Text = "Searching for metadata...";
            EnableControls(false);
            _taskbarItem.SetProgress(0).Indeterminate();
        }

        private void GetMetadataSucceed(IPromise<bool> promise)
        {
            // TODO: Centralize button text
            buttonScan.Text = "Scan";
            textBoxOutput.Text = _controller.Job.OutputPath;
            AppendStatus("Metadata search completed successfully!");
            _taskbarItem.NoProgress();
            RefreshUI();
            EnableControls(true);
            PromptForMetadataIfNeeded();
        }

        private void PromptForMetadataIfNeeded()
        {
            if (!_controller.Job.Movies.Any() && !_controller.Job.TVShows.Any())
            {
                ShowMetadataSearchWindow();
            }
        }

        private void GetMetadataFail(IPromise<bool> promise)
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

            if (promise.LastException != null)
            {
                ShowErrorDialog("Error: Metadata Search Failed", promise.LastException);
            }

            EnableControls(true);
        }

        #endregion

        #region File renamer

        private bool _isRenaming;

        private void RenameAsync()
        {
            if (_controller.Job == null)
                return;

            if (_isRenaming)
                return;

            _isRenaming = true;

            new EmptyPromise(this)
                .Work(p => _controller.RenameSync(null))
                .Done(p => textBoxOutput.Text = _controller.Job.OutputPath)
                .Always(p => _isRenaming = false)
                .Start()
                ;
        }

        #endregion

        #region UI

        private void RefreshUI()
        {
            RefreshPlaylists();
            playlistListView.SelectBestPlaylist();
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
            var isScanning = (_stage == Stage.Scan);
            var isConverting = (_stage == Stage.Convert);

            openBDROMFolderToolStripMenuItem.Enabled = enabled;
            openDiscToolStripMenuItem.Enabled = enabled;
            searchForMetadataToolStripMenuItem.Enabled = enabled && hasJob;

            discInfoToolStripMenuItem.Enabled = (enabled || isConverting) && hasJob;
            filterPlaylistsToolStripMenuItem.Enabled = enabled;
            showAllPlaylistsToolStripMenuItem.Enabled = enabled;
            filterTracksToolStripMenuItem.Enabled = enabled;
            showAllTracksToolStripMenuItem.Enabled = enabled;

//            optionsToolStripMenuItem.Enabled = enabled;

            textBoxInput.ReadOnly = !enabled;
            buttonScan.Enabled = enabled;
            buttonCancelScan.Enabled = !enabled && isScanning;
            rescanToolStripMenuItem.Enabled = enabled;

            textBoxOutput.ReadOnly = !enabled;
            buttonConvert.Enabled = enabled && isPlaylistSelected;
            buttonCancelConvert.Enabled = !enabled && isConverting;
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

        #region Controller timestamping

        private void SetLastControllerEventTimeStamp()
        {
            _lastControllerEvent = DateTime.Now;
        }

        /// <summary>
        /// Gets whether the user is allowed to press the "Scan" or "Convert" buttons.
        /// </summary>
        /// <remarks>
        /// If the user presses "Scan" and "Cancel" in succession too quickly, it can goof up the state of the controller
        /// and/or plugins because the cancel event may get received <i>after</i> the subsequent "Scan" event,
        /// which causes <see cref="ProgressProvider"/> to throw an exception when its <see cref="ProgressProvider.Reset"/>
        /// method is called.  To prevent this from happening, we must enforce a one second delay between actions performed
        /// on the controller.
        /// </remarks>
        private bool PermitScanOrConvert
        {
            get { return DateTime.Now - _lastControllerEvent > TimeSpan.FromSeconds(0.5); }
        }

        #endregion

        #region Scan & Convert stages

        /// <summary>
        /// Asynchronously scans the selected BD-ROM folder.
        /// </summary>
        /// <param name="path">Optional path to the root BD-ROM folder.  If specified, the "Source BD-ROM" textbox will be populated with this path.</param>
        private void Scan(string path = null)
        {
            if (!PermitScanOrConvert)
                return;

            if (path != null)
                textBoxInput.Text = path;

            _controller.SetUIContext(this);

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
            if (!PermitScanOrConvert)
                return;

            var selectedPlaylist = playlistListView.SelectedPlaylist;
            if (selectedPlaylist == null)
            {
                MessageBox.Show(this, "Please select a playlist to mux", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Stop);
                return;
            }

            _controller.Job.SelectedPlaylistIndex = _controller.Job.Disc.Playlists.IndexOf(selectedPlaylist);

            _controller.SetUIContext(this);

            _controller
                .CreateConvertTask(CreateCancellationTokenSource().Token, textBoxOutput.Text)
                .Start();
        }

        #endregion

        private DateTime _lastControllerEvent;

        #region Scan events

        private void ControllerOnBeforeScanStart(IPromise<bool> promise)
        {
            SetLastControllerEventTimeStamp();

            _stage = Stage.Scan;

            buttonScan.Text = "Scanning...";
            textBoxStatus.Text = "Scan started...";
            EnableControls(false);

            _taskbarItem.SetProgress(0).Indeterminate();
        }

        private void ControllerOnScanSucceeded(IPromise<bool> promise)
        {
            SetLastControllerEventTimeStamp();

            textBoxOutput.Text = _controller.Job.OutputPath;
            AppendStatus("Scan succeeded!");

            _state = ProgressProviderState.Success;

            RefreshUI();
            PromptForMetadataIfNeeded();
        }

        private void ControllerOnScanFailed(IPromise<bool> promise)
        {
            SetLastControllerEventTimeStamp();

            if (IsCancellationRequested)
            {
                AppendStatus("Scan canceled!");
                _state = ProgressProviderState.Canceled;
            }
            else
            {
                AppendStatus("Scan failed!");
                _state = ProgressProviderState.Error;
            }
            if (promise.LastException != null)
            {
                ShowErrorDialog("Error: Scan Failed", promise.LastException);
            }
        }

        private void ControllerOnScanCompleted(IPromise<bool> promise)
        {
            SetLastControllerEventTimeStamp();

            _stage = Stage.None;
            
            buttonScan.Text = "Scan";
            EnableControls(true);

            UpdateProgressBars();
        }

        #endregion

        #region Convert events

        private void ControllerOnConvertStarted(IPromise<bool> promise)
        {
            SetLastControllerEventTimeStamp();

            _stage = Stage.Convert;

            buttonConvert.Text = "Converting...";
            AppendStatus("Convert started...");
            EnableControls(false);
            
            _taskbarItem.SetProgress(0).Indeterminate();
        }

        private void ControllerOnConvertSucceeded(IPromise<bool> promise)
        {
            SetLastControllerEventTimeStamp();

            AppendStatus("Convert succeeded!");

            _state = ProgressProviderState.Success;
        }

        private void ControllerOnConvertFailed(IPromise<bool> promise)
        {
            SetLastControllerEventTimeStamp();

            if (IsCancellationRequested)
            {
                AppendStatus("Convert canceled!");
                _state = ProgressProviderState.Canceled;
            }
            else
            {
                AppendStatus("Convert failed!");
                _state = ProgressProviderState.Error;
            }
            if (promise.LastException != null)
            {
                ShowErrorDialog("Error: Convert Failed", promise.LastException);
            }
        }

        private void ControllerOnConvertCompleted(IPromise<bool> promise)
        {
            SetLastControllerEventTimeStamp();

            _stage = Stage.None;
            
            buttonConvert.Text = "Convert";
            EnableControls(true);

            UpdateProgressBars();
        }

        #endregion

        #region Progress events

        private void ControllerOnPluginProgressUpdated(IPlugin plugin, ProgressProvider progressProvider)
        {
            SetLastControllerEventTimeStamp();

            if (!_isRunning && progressProvider.State == ProgressProviderState.Running)
            {
            }
            else
            {
                _state = progressProvider.State;
            }

            if (!_isRunning && _state == ProgressProviderState.Running)
            {
                _logger.Warn("State mismatch: _isRunning == false but _state == Running");
            }

            var percentCompleteStr = (progressProvider.PercentComplete/100.0).ToString("P");
            var line = string.Format("{0} is {1} - {2} complete - {3} - {4} elapsed, {5} remaining",
                                     plugin.Name, progressProvider.State, percentCompleteStr,
                                     progressProvider.Status,
                                     progressProvider.RunTime.ToStringShort(),
                                     progressProvider.TimeRemaining.ToStringShort());

            if (_isRunning)
            {
                AppendStatus(line);
            }

            progressBar.ValuePercent = progressProvider.PercentComplete;
            _progressBarToolTip.SetToolTip(progressBar, string.Format("{0}: {1}", progressProvider.State, percentCompleteStr));
            _taskbarItem.Progress = progressProvider.PercentComplete;

            UpdateProgressBars();
        }

        private void UpdateProgressBars()
        {
            switch (_state)
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
                case ProgressProviderState.Success:
                    progressBar.SetSuccess();
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
            SetLastControllerEventTimeStamp();
            var caption = string.Format("{0} Error", AppUtils.AppName);
            ShowErrorDialog(caption, args.ExceptionObject as Exception);
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

        private void newInstanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void discInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_controller.Job == null)
                return;

            using (var form = new FormDiscInfo(_controller.Job.Disc, _windowMenuFactory))
            {
                form.ShowDialog(this);
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
            FileUtils.OpenFolder(_directoryLocator.LogDir, this);
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if ENABLE_UPDATER
            _updateHelper.Click();
#endif
        }

        private void aboutBDHeroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAboutBox();
        }

        private void forceGarbageCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GC.Collect();
        }

        private void appendDividerToLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _logger.Info("\n\n\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n\n\n");
        }

        private void textEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormTextEditorTest().Show(this);
        }

        private void downloadUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if ENABLE_UPDATER
            _updateHelper.Click();
#endif
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
            RenameAsync();
        }

        private void PlaylistListViewOnItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs args)
        {
            var playlist = playlistListView.SelectedPlaylist;

            _controller.Job.SelectedPlaylist = playlist;
            buttonConvert.Enabled = playlist != null;
            tracksPanel.SetPlaylist(playlist, _controller.Job.Disc.Languages.ToArray());
            chaptersPanel.Playlist = playlist;

            RenameAsync();
        }

        private void PlaylistListViewOnShowAllChanged(object sender, EventArgs eventArgs)
        {
            showAllPlaylistsToolStripMenuItem.Checked = playlistListView.ShowAll;
        }

        private void PlaylistListViewOnPlaylistReconfigured(Playlist playlist)
        {
            RenameAsync();
        }

        private void TracksPanelOnPlaylistReconfigured(Playlist playlist)
        {
            playlistListView.ReconfigurePlaylist(playlist);
            RenameAsync();
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
                && !e.Data.GetDataPresent(typeof(ExternalDragProvider.Format))
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
            textBoxInput.UnHighlight();
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
