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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DotNetUtils.Forms;

namespace WindowsOSUtils.Win32
{
    /// <seealso cref="http://stackoverflow.com/a/4616637/467582"/>
    public partial class SystemMenu
    {
        #region Fields (private)

        private readonly Form _form;

        /// <summary>
        ///     Handle to a copy of the form's system (window) menu.
        /// </summary>
        private readonly IntPtr _hSysMenu;

        private readonly IList<SystemMenuItem> _items = new List<SystemMenuItem>();

        private uint _menuItemIdCounter = 0x1;

        #endregion

        public SystemMenu(Form form, IWndProcObservable wndProcObservable)
        {
            _form = form;
            _hSysMenu = GetSystemMenu(_form.Handle, false);

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
            PInvokeUtils.Try(() => AppendMenu(_hSysMenu, MF_STRING, menuItem.Id, menuItem.Text));
            _items.Add(menuItem);
        }

        public void AppendSeparator()
        {
            PInvokeUtils.Try(() => AppendMenu(_hSysMenu, MF_SEPARATOR, 0, string.Empty));
        }

        public void InsertMenu(uint position, SystemMenuItem menuItem)
        {
            PInvokeUtils.Try(() => InsertMenu(_hSysMenu, position, MF_BYPOSITION | MF_STRING, menuItem.Id, menuItem.Text));
            _items.Add(menuItem);
        }

        public void InsertSeparator(uint position)
        {
            PInvokeUtils.Try(() => InsertMenu(_hSysMenu, position, MF_BYPOSITION | MF_SEPARATOR, 0, string.Empty));
        }

        public void UpdateMenu(SystemMenuItem menuItem)
        {
            var mii = new MENUITEMINFO
                      {
                          fMask = MIIM_CHECKMARKS | MIIM_DATA | MIIM_FTYPE | MIIM_ID | MIIM_STATE | MIIM_STRING
                      };
            mii.cbSize = (uint) Marshal.SizeOf(mii);

            PInvokeUtils.Try(() => GetMenuItemInfo(_hSysMenu, menuItem.Id, false, ref mii));

            if (menuItem.Enabled)
                mii.fState &= (~MFS_DISABLED); // clear "disabled" flag
            else
                mii.fState |= MFS_DISABLED;    // set "disabled" flag

            if (menuItem.Checked)
                mii.fState |= MFS_CHECKED;    // set "checked" flag
            else
                mii.fState &= (~MFS_CHECKED); // clear "checked" flag

            mii.fMask = MIIM_STATE;

            PInvokeUtils.Try(() => SetMenuItemInfo(_hSysMenu, menuItem.Id, false, ref mii));

            // TODO: From my observations, this function always returns false, even though it appears to succeed.
            //       Am I using it incorrectly?
            DrawMenuBar(_hSysMenu);
        }

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
        public readonly uint Id;

        public string Text;
        public bool Enabled = true;
        public bool Checked = false;

        public event EventHandler Clicked;

        internal SystemMenuItem(uint id)
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
