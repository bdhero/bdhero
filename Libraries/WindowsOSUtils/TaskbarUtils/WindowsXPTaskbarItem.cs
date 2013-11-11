using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OSUtils.TaskbarUtils;

namespace WindowsOSUtils.TaskbarUtils
{
    public class WindowsXPTaskbarItem : ITaskbarItem
    {
        public double Progress { get; set; }

        public ITaskbarItem SetOverlayIcon(Icon icon, string accessibilityText)
        {
            return this;
        }

        public ITaskbarItem NoProgress()
        {
            return this;
        }

        public ITaskbarItem Indeterminate()
        {
            return this;
        }

        public ITaskbarItem Normal()
        {
            return this;
        }

        public ITaskbarItem Error()
        {
            return this;
        }

        public ITaskbarItem Pause()
        {
            return this;
        }

        public ITaskbarItem SetProgress(double percent)
        {
            Progress = percent;
            return this;
        }
    }
}
