// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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
using System.Windows.Forms;

namespace DotNetUtils.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ListViewItem"/> controls.
    /// </summary>
    public static class ListViewItemExtensions
    {
        /// <summary>
        /// Sets or appends the specified text to this ListViewItem's <see cref="ListViewItem.ToolTipText"/> property.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="text"></param>
        public static void AppendToolTip(this ListViewItem item, string text)
        {
            item.ToolTipText = string.IsNullOrEmpty(item.ToolTipText) ? text : string.Format("{0}; {1}", item.ToolTipText, text);
        }

        public static void MarkBestChoice(this ListViewItem item)
        {
            item.Font = new Font(item.Font, item.Font.Style & ~FontStyle.Regular | FontStyle.Bold);
        }

        public static void VisuallyDisable(this ListViewItem item)
        {
            item.ForeColor = SystemColors.GrayText;
            item.Font = new Font(item.Font, item.Font.Style & ~FontStyle.Regular | FontStyle.Strikeout);
        }

        public static void MarkHidden(this ListViewItem item)
        {
            item.Font = new Font(item.Font, item.Font.Style & ~FontStyle.Regular | FontStyle.Italic);
            item.Text += " *";
        }
    }
}