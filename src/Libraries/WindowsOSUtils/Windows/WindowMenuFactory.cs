using System;
using System.Windows.Forms;
using DotNetUtils.Annotations;
using OSUtils.Window;

namespace WindowsOSUtils.Windows
{
    [UsedImplicitly]
    public class WindowMenuFactory : IWindowMenuFactory
    {
        private uint _menuItemIdCounter = 0x1;

        public IWindowMenu CreateMenu(Form form)
        {
            return new WindowMenu(form);
        }

        public IWindowMenuItem CreateMenuItem(string text = null, EventHandler clickHandler = null)
        {
            var menuItem = new WindowMenuItem(_menuItemIdCounter++) { Text = text };

            if (clickHandler != null)
            {
                menuItem.Clicked += clickHandler;
            }

            return menuItem;
        }
    }
}
