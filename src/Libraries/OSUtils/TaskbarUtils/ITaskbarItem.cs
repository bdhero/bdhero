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

using System.Drawing;

namespace OSUtils.TaskbarUtils
{
    /// <summary>
    /// Interface for a Windows taskbar item (program icon) capable of displaying progress information.
    /// </summary>
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
