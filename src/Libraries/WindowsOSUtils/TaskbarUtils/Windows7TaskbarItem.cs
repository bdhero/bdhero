using System;
using System.Drawing;
using Microsoft.WindowsAPICodePack.Taskbar;
using OSUtils;
using OSUtils.TaskbarUtils;

namespace WindowsOSUtils.TaskbarUtils
{
    /// <summary>
    /// Mono-safe wrapper for <see cref="Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager"/>.
    /// Requires Windows 7 or higher.
    /// </summary>
    public class Windows7TaskbarItem : ITaskbarItem
    {
        private const double MaxValue = 100.0;

        /// <summary>
        /// Number of decimal places of precision to provide when displaying progress percentage.
        /// </summary>
        private const int DecimalPlaces = 3;

        private static double Multiplier
        {
            get { return Math.Pow(10, DecimalPlaces); }
        }

        private readonly TaskbarManager _taskbarManager;
        private readonly IntPtr _windowHandle;

        private double _progress;

        public static bool IsPlatformSupported
        {
            get { return SystemInfo.Instance.OS.Type == OSType.Windows && TaskbarManager.IsPlatformSupported; }
        }

        public double Progress
        {
            get { return GetProgress(); }
            set { SetProgress(value); }
        }

        /// <summary>
        /// Constructs a new Windows 7 taskbar item object.
        /// </summary>
        /// <param name="windowHandle">Window handle.</param>
        /// <exception cref="PlatformNotSupportedException">
        ///     Thrown if the operating system is not Windows 7 or higher.
        /// </exception>
        public Windows7TaskbarItem(IntPtr windowHandle)
        {
            if (!IsPlatformSupported)
                throw new PlatformNotSupportedException("This class requires Windows 7 or higher");

#if !__MonoCS__
            _taskbarManager = TaskbarManager.Instance;
            _windowHandle = windowHandle;
#endif
        }

        public ITaskbarItem SetOverlayIcon(Icon icon, string accessibilityText)
        {
#if !__MonoCS__
            _taskbarManager.SetOverlayIcon(_windowHandle, icon, accessibilityText);
#endif
            return this;
        }

        public ITaskbarItem NoProgress()
        {
            return SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        public ITaskbarItem Indeterminate()
        {
            return SetProgressState(TaskbarProgressBarState.Indeterminate);
        }

        public ITaskbarItem Normal()
        {
            return SetProgressState(TaskbarProgressBarState.Normal);
        }

        public ITaskbarItem Error()
        {
            return SetProgressState(TaskbarProgressBarState.Error);
        }

        public ITaskbarItem Pause()
        {
            return SetProgressState(TaskbarProgressBarState.Paused);
        }

        public ITaskbarItem SetProgress(double percent)
        {
#if !__MonoCS__
            var currentValue = (int) (percent * Multiplier);
            var maximumValue = (int) (MaxValue * Multiplier);
            _taskbarManager.SetProgressValue(currentValue, maximumValue, _windowHandle);
#endif
            _progress = percent;
            return this;
        }

        private double GetProgress()
        {
            return _progress;
        }

        private ITaskbarItem SetProgressState(TaskbarProgressBarState state)
        {
#if !__MonoCS__
            _taskbarManager.SetProgressState(state, _windowHandle);
#endif
            return this;
        }
    }
}
