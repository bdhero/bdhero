using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSUtils.TaskbarUtils
{
    public interface ITaskbarItemFactory
    {
        /// <summary>
        /// Returns a singleton taskbar item instance for the given window handle.
        /// </summary>
        /// <param name="windowHandle">HWND pointer to a window</param>
        /// <returns>ITaskbarItem instance for the given window</returns>
        ITaskbarItem GetInstance(IntPtr windowHandle);
    }
}
