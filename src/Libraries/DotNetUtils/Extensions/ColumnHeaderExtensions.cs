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

using System;
using System.Windows.Forms;

namespace DotNetUtils.Extensions
{
    public static class ColumnHeaderExtensions
    {
        public static void AutoResizeHeader(this ColumnHeader columnHeader)
        {
            columnHeader.Width = -2;
        }

        public static void AutoResizeContent(this ColumnHeader columnHeader)
        {
            columnHeader.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        public static void AutoResize(this ColumnHeader columnHeader)
        {
            AutoResizeHeader(columnHeader);
            var before = columnHeader.Width;
            AutoResizeContent(columnHeader);
            var after = columnHeader.Width;
            columnHeader.Width = Math.Max(before, after);
        }
    }
}