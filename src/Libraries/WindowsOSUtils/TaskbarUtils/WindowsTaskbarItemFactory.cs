using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSUtils.TaskbarUtils;

namespace WindowsOSUtils.TaskbarUtils
{
    /// <summary>
    /// Thread-safe taskbar item factory.
    /// </summary>
    public class WindowsTaskbarItemFactory : ITaskbarItemFactory
    {
        private readonly ConcurrentDictionary<IntPtr, ITaskbarItem> _taskbarItems =
            new ConcurrentDictionary<IntPtr, ITaskbarItem>();

        public ITaskbarItem GetInstance(IntPtr windowHandle)
        {
            return _taskbarItems.GetOrAdd(windowHandle, ValueFactory);
        }

        private static ITaskbarItem ValueFactory(IntPtr windowHandle)
        {
            if (Windows7TaskbarItem.IsPlatformSupported)
                return new Windows7TaskbarItem(windowHandle);
            return new MockTaskbarItem();
        }
    }
}
