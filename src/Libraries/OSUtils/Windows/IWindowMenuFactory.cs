using System;
using DotNetUtils.Forms;

namespace OSUtils.Windows
{
    public interface IWindowMenuFactory
    {
        IWindowMenu CreateMenu(WndProcObservableForm form);

        IWindowMenuItem CreateMenuItem(string text = null, EventHandler clickHandler = null);
    }
}
