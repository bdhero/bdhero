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
using System.Drawing;
using Microsoft.WindowsAPICodePack.Taskbar;
using OSUtils;
using OSUtils.Info;
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
