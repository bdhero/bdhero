using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using DotNetUtils.Forms;

namespace WindowsOSUtils.Win32
{
    /// <seealso cref="http://stackoverflow.com/a/4616637/467582"/>
    public class SystemMenu
    {
        #region P/Invoke constants

        #region uFlags - all methods

        /// <summary>
        ///     A window receives this message when the user chooses a command from the Window menu (formerly known
        ///     as the system or control menu) or when the user chooses the maximize button, minimize button,
        ///     restore button, or close button.
        /// </summary>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/ms646360(v=vs.85).aspx"/>
        private const int WM_SYSCOMMAND = 0x00000112;

        /// <summary>
        ///     Uses a bitmap as the menu item. The lpNewItem parameter contains a handle to the bitmap.
        /// </summary>
        private const int MF_BITMAP = 0x00000004;

        /// <summary>
        ///     Places a check mark next to the menu item. If the application provides check-mark bitmaps (see SetMenuItemBitmaps, this flag displays the check-mark bitmap next to the menu item.
        /// </summary>
        private const int MF_CHECKED = 0x00000008;

        /// <summary>
        ///     Disables the menu item so that it cannot be selected, but the flag does not gray it.
        /// </summary>
        private const int MF_DISABLED = 0x00000002;

        /// <summary>
        ///     Enables the menu item so that it can be selected, and restores it from its grayed state.
        /// </summary>
        private const int MF_ENABLED = 0x00000000;

        /// <summary>
        ///     Disables the menu item and grays it so that it cannot be selected.
        /// </summary>
        private const int MF_GRAYED = 0x00000001;

        /// <summary>
        ///     Functions the same as the MF_MENUBREAK flag for a menu bar. For a drop-down menu, submenu, or shortcut menu, the new column is separated from the old column by a vertical line.
        /// </summary>
        private const int MF_MENUBARBREAK = 0x00000020;

        /// <summary>
        ///     Places the item on a new line (for a menu bar) or in a new column (for a drop-down menu, submenu, or shortcut menu) without separating columns.
        /// </summary>
        private const int MF_MENUBREAK = 0x00000040;

        /// <summary>
        ///     Specifies that the item is an owner-drawn item. Before the menu is displayed for the first time,
        ///     the window that owns the menu receives a <c>WM_MEASUREITEM</c> message to retrieve the width and height
        ///     of the menu item. The <c>WM_DRAWITEM</c> message is then sent to the window procedure of the owner
        ///     window whenever the appearance of the menu item must be updated.
        /// </summary>
        private const int MF_OWNERDRAW = 0x00000100;

        /// <summary>
        ///     Specifies that the menu item opens a drop-down menu or submenu. The <c>uIDNewItem</c> parameter
        ///     specifies a handle to the drop-down menu or submenu. This flag is used to add a menu name to a menu bar,
        ///     or a menu item that opens a submenu to a drop-down menu, submenu, or shortcut menu.
        /// </summary>
        private const int MF_POPUP = 0x00000010;

        /// <summary>
        ///     Draws a horizontal dividing line. This flag is used only in a drop-down menu, submenu, or shortcut menu.
        ///     The line cannot be grayed, disabled, or highlighted. The <c>lpNewItem</c> and <c>uIDNewItem</c>
        ///     parameters are ignored.
        /// </summary>
        private const int MF_SEPARATOR = 0x00000800;

        /// <summary>
        ///     Specifies that the menu item is a text string; the <c>lpNewItem</c> parameter is a pointer to the string.
        /// </summary>
        private const int MF_STRING = 0x00000000;

        /// <summary>
        ///     Does not place a check mark next to the item (default). If the application supplies check-mark bitmaps
        ///     (see <c>SetMenuItemBitmaps</c>), this flag displays the clear bitmap next to the menu item.
        /// </summary>
        private const int MF_UNCHECKED = 0x00000000;

        #endregion

        #region uFlags - InsertMenu() specific

        /// <summary>
        ///     Indicates that the <c>uPosition</c> parameter gives the identifier of the menu item.
        ///     The <see cref="MF_BYCOMMAND"/> flag is the default if neither the <see cref="MF_BYCOMMAND"/> nor
        ///     <see cref="MF_BYPOSITION"/> flag is specified.
        /// </summary>
        private const int MF_BYCOMMAND = 0x00000000;

        /// <summary>
        ///     Indicates that the <c>uPosition</c> parameter gives the zero-based relative position of the new menu item.
        ///     If <c>uPosition</c> is <c>-1</c>, the new menu item is appended to the end of the menu.
        /// </summary>
        private const int MF_BYPOSITION = 0x00000400;

        #endregion

        #endregion

        #region P/Invoke declarations

        /// <summary>
        ///     Enables the application to access the window menu (also known as the system menu or the control menu)
        ///     for copying and modifying.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window that will own a copy of the window menu.
        /// </param>
        /// <param name="bRevert">
        ///     <para>The action to be taken.</para>
        ///     <para>
        ///         If this parameter is <c>false</c>, <see cref="GetSystemMenu"/> returns a handle
        ///         to the copy of the window menu currently in use. The copy is initially identical to the window menu,
        ///         but it can be modified.
        ///     </para>
        ///     <para>
        ///         If this parameter is <c>true</c>, <see cref="GetSystemMenu"/> resets the window menu back to the default state.
        ///         The previous window menu, if any, is destroyed.
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         If the <paramref name="bRevert"/> parameter is <c>false</c>, the return value is a handle to a copy
        ///         of the window menu.
        ///     </para>
        ///     <para>
        ///         If the <paramref name="bRevert"/> parameter is <c>true</c>, the return value is <c>null</c>.
        ///     </para>
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Any window that does not use the <see cref="GetSystemMenu"/> function to make its own copy of the
        ///         window menu receives the standard window menu.
        ///     </para>
        ///     <para>
        ///         The window menu initially contains items with various identifier values, such as
        ///         <c>SC_CLOSE</c>, <c>SC_MOVE</c>, and <c>SC_SIZE</c>.
        ///     </para>
        ///     <para>
        ///         Menu items on the window menu send <c>WM_SYSCOMMAND</c> messages.
        ///     </para>
        ///     <para>
        ///         All predefined window menu items have identifier numbers greater than <c>0xF000</c>.
        ///         If an application adds commands to the window menu, it should use identifier numbers less than
        ///         <c>0xF000</c>.
        ///     </para>
        ///     <para>
        ///         The system automatically grays items on the standard window menu, depending on the situation.
        ///         The application can perform its own checking or graying by responding to the <c>WM_INITMENU</c>
        ///         message that is sent before any menu is displayed.
        ///     </para>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        ///     Appends a new item to the end of the specified menu bar, drop-down menu, submenu, or shortcut menu.
        ///     You can use this function to specify the content, appearance, and behavior of the menu item.
        /// </summary>
        /// <param name="hMenu">
        ///     A handle to the menu bar, drop-down menu, submenu, or shortcut menu to be changed.
        /// </param>
        /// <param name="uFlags">
        ///     Controls the appearance and behavior of the new menu item. This parameter can be a combination of
        ///     <c>MF_</c> values.
        /// </param>
        /// <param name="uIDNewItem">
        ///     The identifier of the new menu item or, if the <paramref name="uFlags"/> parameter is set to
        ///     <see cref="MF_POPUP"/>, a handle to the drop-down menu or submenu.
        /// </param>
        /// <param name="lpNewItem">
        ///     The content of the new menu item. The interpretation of <paramref name="lpNewItem"/> depends on whether
        ///     the <paramref name="uFlags"/> parameter includes the following values:
        ///     <see cref="MF_BITMAP"/>, <see cref="MF_OWNERDRAW"/>, <see cref="MF_STRING"/>.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero.
        ///     To get extended error information, call <c>GetLastError</c>.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The application must call the DrawMenuBar function whenever a menu changes, whether the menu is in
        ///         a displayed window.
        ///     </para>
        ///     <para>
        ///         To get keyboard accelerators to work with bitmap or owner-drawn menu items, the owner of the menu
        ///         must process the <c>WM_MENUCHAR</c> message.
        ///     </para>
        ///     <para>The following groups of flags cannot be used together:</para>
        ///     <list>
        ///         <item><see cref="MF_BITMAP"/>, <see cref="MF_STRING"/>, and <see cref="MF_OWNERDRAW"/></item>
        ///         <item><see cref="MF_CHECKED"/> and <see cref="MF_UNCHECKED"/></item>
        ///         <item><see cref="MF_DISABLED"/>, <see cref="MF_ENABLED"/>, and <see cref="MF_GRAYED"/></item>
        ///         <item><see cref="MF_MENUBARBREAK"/> and <see cref="MF_MENUBREAK"/></item>
        ///     </list>
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);

        /// <summary>
        ///     <para>
        ///         Inserts a new menu item into a menu, moving other items down the menu.
        ///     </para>
        ///     <para>
        ///         <b>Note</b>: The <see cref="InsertMenu(System.IntPtr,int,int,int,string)"/> function has been superseded by the
        ///         <see cref="InsertMenuItem"/> function.
        ///         You can still use <see cref="InsertMenu(System.IntPtr,int,int,int,string)"/>, however, if you do not need any of the extended features
        ///         of <see cref="InsertMenuItem"/>.
        ///     </para>
        /// </summary>
        /// <param name="hMenu">
        ///     A handle to the menu to be changed.
        /// </param>
        /// <param name="uPosition">
        ///     The menu item before which the new menu item is to be inserted, as determined by the
        ///     <paramref name="uFlags"/> parameter.
        /// </param>
        /// <param name="uFlags">
        ///     Controls the interpretation of the <paramref name="uPosition"/> parameter and the content, appearance,
        ///     and behavior of the new menu item. This parameter must include either
        ///     <see cref="MF_BYCOMMAND"/> or <see cref="MF_BYPOSITION"/>.
        /// </param>
        /// <param name="uIDNewItem">
        ///     The identifier of the new menu item or, if the <paramref name="uFlags"/> parameter has the
        ///     <see cref="MF_POPUP"/> flag set, a handle to the drop-down menu or submenu.
        /// </param>
        /// <param name="lpNewItem">
        ///     The content of the new menu item. The interpretation of <paramref name="lpNewItem"/> depends on whether
        ///     the <paramref name="uFlags"/> parameter includes the
        ///     <see cref="MF_BITMAP"/>, <see cref="MF_OWNERDRAW"/>, or <see cref="MF_STRING"/> flag.
        /// </param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

        #endregion

        #region Fields (private)

        /// <summary>
        ///     Handle to a copy of the form's system (window) menu.
        /// </summary>
        private readonly IntPtr _hSysMenu;

        private readonly IList<SystemMenuItem> _items = new List<SystemMenuItem>();

        #endregion

        public SystemMenu(Form form, IWndProcObservable wndProcObservable)
        {
            _hSysMenu = GetSystemMenu(form.Handle, false);
            wndProcObservable.WndProcMessage += OnWndProcMessage;
        }

        #region Win32 message handling

        private void OnWndProcMessage(ref Message m)
        {
            // Test if the About item was selected from the system menu
            if (m.Msg != WM_SYSCOMMAND)
                return;

            var itemId = (int) m.WParam;
            var item = _items.FirstOrDefault(menuItem => menuItem.Id == itemId);

            if (item == null)
                return;

            item.Click(EventArgs.Empty);
        }

        #endregion

        #region Public API

        public void AppendMenu(SystemMenuItem menuItem)
        {
            AppendMenu(_hSysMenu, MF_STRING, menuItem.Id, menuItem.Text);
            _items.Add(menuItem);
        }

        public void AppendSeparator()
        {
            AppendMenu(_hSysMenu, MF_SEPARATOR, 0, string.Empty);
        }

        public void InsertMenu(int position, SystemMenuItem menuItem)
        {
            InsertMenu(_hSysMenu, position, MF_BYPOSITION | MF_STRING, menuItem.Id, menuItem.Text);
            _items.Add(menuItem);
        }

        public void InsertSeparator(int position)
        {
            InsertMenu(_hSysMenu, position, MF_BYPOSITION | MF_SEPARATOR, 0, string.Empty);
        }

        private int _menuItemIdCounter = 0x1;

        public SystemMenuItem CreateMenuItem(string text = null, EventHandler clickHandler = null)
        {
            var menuItem = new SystemMenuItem(_menuItemIdCounter++) { Text = text };

            if (clickHandler != null)
            {
                menuItem.Clicked += clickHandler;
            }

            return menuItem;
        }

        #endregion
    }

    public class SystemMenuItem
    {
        public readonly int Id;

        public string Text;

        public event EventHandler Clicked;

        internal SystemMenuItem(int id)
        {
            Id = id;
        }

        public void Click(EventArgs e)
        {
            if (Clicked != null)
            {
                Clicked(this, e);
            }
        }
    }
}
