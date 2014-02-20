using System;
using DotNetUtils.Forms;

namespace OSUtils.Window
{
    public interface IWindowMenuFactory
    {
        IWindowMenu CreateMenu(WndProcObservableForm form);

        IWindowMenuItem CreateMenuItem(string text = null, EventHandler clickHandler = null);
    }
}
