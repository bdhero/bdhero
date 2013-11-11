using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using BDHero.Utils;
using DotNetUtils.Annotations;
using DotNetUtils.Forms;
using DotNetUtils.TaskUtils;
using OSUtils.DriveDetector;

namespace BDHeroGUI.Components
{
    [DefaultProperty("NoDiscText")]
    [DefaultEvent("DiscSelected")]
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.StatusStrip)]
    [ToolboxItem(true)]
    public partial class DiscMenu : ToolStripMenuItem
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Default text values

        private const string DefaultNoDiscText = "No discs found";
        private const string DefaultScanningText = "Scanning for discs...";

        #endregion

        #region Public members

        [Category("Appearance")]
        [Description("The text that is displayed when no discs are found.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Localizable(true)]
        [DefaultValue(DefaultNoDiscText)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string NoDiscText
        {
            get { return _noDiscItem.Text; }
            set { _noDiscItem.Text = value; }
        }

        [Category("Appearance")]
        [Description("The text that is displayed while the menu is scanning for discs.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Localizable(true)]
        [DefaultValue(DefaultScanningText)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string ScanningText
        {
            get { return _scanningItem.Text; }
            set { _scanningItem.Text = value; }
        }

        [Description("Invoked whenever a disc is selected (clicked on) by the user.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public event DiscMenuItemClickHandler DiscSelected;

        #endregion

        /// <summary>
        /// Gets a collection of all BD-ROM drives connected to the host OS.
        /// </summary>
        private static DriveInfo[] Drives
        {
            get { return DriveInfo.GetDrives().Where(BDFileUtils.IsBDROM).ToArray(); }
        }

        /// <summary>
        /// Gets ALL menu items present in the dropdown list.
        /// </summary>
        private ToolStripItem[] AllMenuItems
        {
            get { return DropDownItems.OfType<ToolStripItem>().ToArray(); }
        }

        private readonly ToolStripMenuItem _dummyItem = new ToolStripMenuItem("DUMMY") { Enabled = false };
        private readonly ToolStripMenuItem _noDiscItem = new ToolStripMenuItem(DefaultNoDiscText) { Enabled = false };
        private readonly ToolStripMenuItem _scanningItem = new ToolStripMenuItem(DefaultScanningText) { Enabled = false };
        private readonly ToolStripSeparator _dividerItem = new ToolStripSeparator();

        private IDriveDetector _detector;
        private bool _isScanning;
        private bool _isInitialized;

        #region Constructors

        public DiscMenu()
        {
            InitializeComponent();
        }

        public DiscMenu(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the <see cref="BDHeroGUI.Components.DiscMenu"/> for use.
        /// </summary>
        /// <param name="observable">
        /// Windows Forms control (typically a <see cref="Form"/>) to listen for <see cref="Form.WndProc"/> events on.
        /// </param>
        /// <param name="detector">
        /// Drive detector.
        /// </param>
        /// <exception cref="InvalidOperationException">Thrown if this method is called more than once.</exception>
        public void Initialize(IWndProcObservable observable, IDriveDetector detector)
        {
            if (_isInitialized)
            {
                throw new InvalidOperationException("DiscMenu has already been initialized");
            }

            observable.WndProcMessage += WndProc;

            _detector = detector;
            _detector.DeviceArrived += OnDeviceArrived;
            _detector.DeviceRemoved += OnDeviceRemoved;

            DropDownOpened += OnDropDownOpened;
            DropDownClosed += OnDropDownClosed;

            Refresh();

            _isInitialized = true;
        }

        #endregion

        #region Event handlers

        private void OnDeviceArrived(object sender, DriveDetectorEventArgs driveDetectorEventArgs)
        {
            Refresh();
        }

        private void OnDeviceRemoved(object sender, DriveDetectorEventArgs driveDetectorEventArgs)
        {
            Refresh();
        }

        private void OnDropDownOpened(object sender, EventArgs eventArgs)
        {
            Refresh();
        }

        private void OnDropDownClosed(object sender, EventArgs eventArgs)
        {
        }

        private void WndProc(ref Message m)
        {
            _detector.WndProc(ref m);
        }

        private void MenuItemOnClick(object sender, EventArgs eventArgs)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem == null) return;

            var driveInfo = menuItem.Tag as DriveInfo;
            if (driveInfo == null) return;

            if (DiscSelected != null)
            {
                DiscSelected(driveInfo);
            }
        }

        #endregion

        #region Disc scanning and menu population logic

        private void Refresh()
        {
            ScanAsync();
        }

        private void ScanAsync()
        {
            if (_isScanning)
            {
                Logger.Debug("Already scanning for discs; ignoring");
                return;
            }

            Logger.Debug("Scanning for discs...");

            _isScanning = true;

            DropDownItems.Add(_dividerItem);
            DropDownItems.Add(_scanningItem);

            var menuItems = new ToolStripItem[0];

            new TaskBuilder()
                .OnCurrentThread()
                .DoWork((invoker, token) => menuItems = CreateToolStripItems(Drives))
                .Succeed(() => UpdateMenu(menuItems))
                .Fail(args => Logger.Error("Error occurred while scanning for discs", args.Exception))
                .Finally(() => _isScanning = false)
                .Build()
                .Start();
        }

        private void ClearMenu()
        {
            // We need to always keep at least 1 menu item in the dropdown list
            // to prevent the list from being positioned in the upper-left corner
            // of the screen.
            DropDownItems.Add(_dummyItem);

            // Special menu items that should NOT be destroyed
            var specialItems = new ToolStripItem[] { _noDiscItem, _scanningItem, _dividerItem };

            // Disc Drive menu items
            var destroyableItems = AllMenuItems.Except(specialItems).Except(new[] { _dummyItem }).ToArray();

            foreach (var menuItem in destroyableItems)
            {
                DestroyMenuItem(menuItem);
                DropDownItems.Remove(menuItem);
            }

            foreach (var menuItem in specialItems)
            {
                DropDownItems.Remove(menuItem);
            }
        }

        private void UpdateMenu(ToolStripItem[] menuItems)
        {
            Logger.DebugFormat("Found {0} discs", menuItems.Length);

            var selectionState = new MenuSelectionState(AllMenuItems);

            ClearMenu();
            PopulateMenuSync(menuItems);

            selectionState.Restore(AllMenuItems);
        }

        private void PopulateMenuSync(ToolStripItem[] items)
        {
            DropDownItems.AddRange(items);

            var menuItems = AllMenuItems.Except(new[] { _dummyItem }).ToArray();

            if (!menuItems.Any())
            {
                DropDownItems.Add(_noDiscItem);
            }

            // We need to always keep at least 1 menu item in the dropdown list
            // to prevent the list from being positioned in the upper-left corner
            // of the screen.
            DropDownItems.Remove(_dummyItem);
        }

        #endregion

        #region Menu item creation / destruction

        private ToolStripItem[] CreateToolStripItems(DriveInfo[] drives)
        {
            return drives.Select(TryCreateMenuItem).Where(item => item != null).ToArray();
        }

        [CanBeNull]
        private ToolStripItem TryCreateMenuItem(DriveInfo driveInfo)
        {
            try { return CreateMenuItem(driveInfo); }
            catch (Exception e) { Logger.InfoFormat("Ignoring exception: {0}", e); }
            return null;
        }

        /// <exception cref="IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
        /// <exception cref="DriveNotFoundException">The drive is not mapped or does not exist.</exception>
        /// <exception cref="SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">The volume label is being set on a network or CD-ROM drive.-or-Access to the drive information is denied.</exception>
        private ToolStripItem CreateMenuItem(DriveInfo driveInfo)
        {
            var driveLetter = driveInfo.Name;
            var text = string.Format("{0} {1}", driveLetter, driveInfo.VolumeLabel);
            var menuItem = new ToolStripMenuItem(text) { Tag = driveInfo };
            menuItem.Click += MenuItemOnClick;
            return menuItem;
        }

        private void DestroyMenuItem(ToolStripItem menuItem)
        {
            menuItem.Tag = null;
            menuItem.Click -= MenuItemOnClick;
        }

        #endregion

        #region Selection state management

        /// <summary>
        /// Captures and restores the selection state (i.e., which item is selected) of a dropdown menu.
        /// </summary>
        private class MenuSelectionState
        {
            private readonly int? _selectedIndex;
            private readonly DriveInfo _selectedDrive;

            /// <summary>
            /// Constructs a new <see cref="MenuSelectionState"/> object and captures the selection state
            /// (i.e., which menu item is selected, if any) from the given menu items.
            /// </summary>
            /// <param name="oldMenuItems">Items present in the dropdown menu <b>before</b> being repopulated.</param>
            public MenuSelectionState(ToolStripItem[] oldMenuItems)
            {
                var selectedItem = oldMenuItems.FirstOrDefault(item => item.Selected);
                if (selectedItem == null) return;

                _selectedIndex = oldMenuItems.ToList().IndexOf(selectedItem);
                _selectedDrive = selectedItem.Tag as DriveInfo;
            }

            /// <summary>
            /// Selects the same menu item that was selected previously.
            /// If the previously selected item is no longer in the list,
            /// the item at the same index is selected.
            /// </summary>
            /// <param name="newMenuItems">Items present in the dropdown menu <b>after</b> being repopulated.</param>
            public void Restore(ToolStripItem[] newMenuItems)
            {
                if (RestoreSelectedDrive(newMenuItems)) return;
                if (RestoreSelectedIndex(newMenuItems)) return;
            }

            private bool RestoreSelectedDrive(ToolStripItem[] newMenuItems)
            {
                if (_selectedDrive == null) return false;

                var itemToSelect = newMenuItems.FirstOrDefault(item => _selectedDrive.IsEqualTo(item));
                if (itemToSelect == null) return false;

                itemToSelect.Select();

                return true;
            }

            private bool RestoreSelectedIndex(ToolStripItem[] newMenuItems)
            {
                if (!_selectedIndex.HasValue) return false;
                if (!newMenuItems.Any()) return false;
                if (_selectedIndex < 0) return false;

                if (_selectedIndex < newMenuItems.Length)
                {
                    newMenuItems[_selectedIndex.Value].Select();
                }
                else
                {
                    newMenuItems.Last().Select();
                }

                return true;
            }
        }

        #endregion
    }

    internal static class DriveInfoExtensions
    {
        public static bool IsEqualTo(this DriveInfo thisDrive, ToolStripItem item)
        {
            if (item == null || thisDrive == null) return false;
            return thisDrive.IsEqualTo(item.Tag as DriveInfo);
        }

        private static bool IsEqualTo(this DriveInfo drive1, DriveInfo drive2)
        {
            if (drive1 == null || drive2 == null) return false;
            return string.Equals(drive1.Name, drive2.Name);
        }
    }

    public delegate void DiscMenuItemClickHandler(DriveInfo driveInfo);
}
