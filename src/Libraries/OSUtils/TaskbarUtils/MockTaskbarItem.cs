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
    public class MockTaskbarItem : ITaskbarItem
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
