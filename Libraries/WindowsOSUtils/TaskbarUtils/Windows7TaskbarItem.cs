﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.WindowsAPICodePack.Taskbar;
using OSUtils;
using OSUtils.TaskbarUtils;

namespace WindowsOSUtils.TaskbarUtils
{
    /// <summary>
    /// XP-safe static wrapper for Microsoft.WindowsAPICodePack.Taskbar.TaskbarManager
    /// </summary>
    public class Windows7TaskbarItem : ITaskbarItem
    {
        private const double MinValue = 0.0;
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

        public Windows7TaskbarItem(IntPtr windowHandle)
        {
            if (!IsPlatformSupported)
                throw new PlatformNotSupportedException("This class requires Windows 7 or higher");

            _taskbarManager = TaskbarManager.Instance;
            _windowHandle = windowHandle;
        }

        public ITaskbarItem SetOverlayIcon(Icon icon, string accessibilityText)
        {
            _taskbarManager.SetOverlayIcon(_windowHandle, icon, accessibilityText);
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
            var currentValue = (int) (percent * Multiplier);
            var maximumValue = (int) (MaxValue * Multiplier);
            _taskbarManager.SetProgressValue(currentValue, maximumValue, _windowHandle);
            _progress = percent;
            return this;
        }

        private double GetProgress()
        {
            return _progress;
        }

        private ITaskbarItem SetProgressState(TaskbarProgressBarState state)
        {
            _taskbarManager.SetProgressState(state, _windowHandle);
            return this;
        }
    }
}
