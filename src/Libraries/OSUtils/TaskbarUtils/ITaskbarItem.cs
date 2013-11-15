using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OSUtils.TaskbarUtils
{
    public interface ITaskbarItem
    {
        /// <summary>
        /// Gets or sets the progress value from 0.0 to 100.0.
        /// </summary>
        double Progress { get; set; }

        ITaskbarItem SetOverlayIcon(Icon icon, string accessibilityText);

        ITaskbarItem NoProgress();
        ITaskbarItem Indeterminate();
        ITaskbarItem Normal();
        ITaskbarItem Error();
        ITaskbarItem Pause();

        /// <summary>
        /// Sets the progress value (percent completed) of this taskbar item.
        /// </summary>
        /// <param name="percent">Progress value from 0.0 to 100.0</param>
        /// <returns>Reference to this taskbar item</returns>
        ITaskbarItem SetProgress(double percent);
    }
}
