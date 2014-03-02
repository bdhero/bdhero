using System;
using System.Windows.Forms;

namespace OSUtils.Window
{
    public interface IWindowMenuFactory
    {
        IWindowMenu CreateMenu(Form form);

        IWindowMenuItem CreateMenuItem(string text = null, EventHandler clickHandler = null);
    }
}
