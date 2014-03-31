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
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DotNetUtils.Extensions;
using NativeAPI.Win.CommonControls;
using NativeAPI.Win.User;

namespace UILib.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ListView"/>s.
    /// </summary>
    public static class ListViewExtensions
    {
        /// <summary>
        /// Automatically resizes each column to fit its header <b>and</b> contents, whichever is wider.
        /// </summary>
        /// <param name="listView"></param>
        public static void AutoSizeColumns(this ListView listView)
        {
#if !__MonoCS__
            listView.SuspendDrawing();

            listView.Columns.OfType<ColumnHeader>().ForEach(header => header.AutoResize());

            listView.ResumeDrawing();
#endif
        }

        /// <summary>
        /// Automatically resizes the last column to take up all available free space.
        /// </summary>
        /// <param name="listView"></param>
        public static void AutoSizeLastColumn(this ListView listView)
        {
#if !__MonoCS__
            if (listView.AnyParent(parent => parent.Disposing || parent.IsDisposed))
                return;

            var columnHeaders = listView.Columns.OfType<ColumnHeader>().ToArray();
            if (!columnHeaders.Any())
                return;

            var maxDisplayIndex = columnHeaders.Max(header => header.DisplayIndex);
            var lastColumn = columnHeaders.LastOrDefault(header => header.DisplayIndex == maxDisplayIndex);
            if (lastColumn != null)
            {
                listView.SuspendDrawing();
                lastColumn.AutoResize();
                lastColumn.Width -= 2; // TODO: Figure out why this is necessary on some ListViews (e.g., FormFileNamerPreferences)
                listView.ResumeDrawing();
            }
#endif
        }

        /// <summary>
        /// Selects all <see cref="ListViewItem"/>s.
        /// </summary>
        /// <param name="listView"></param>
        public static void SelectAll(this ListView listView)
        {
            SelectWhere(listView, item => true);
        }

        /// <summary>
        /// Deselects all <see cref="ListViewItem"/>s.
        /// </summary>
        /// <param name="listView"></param>
        public static void SelectNone(this ListView listView)
        {
            SelectWhere(listView, item => false);
        }

        /// <summary>
        /// Selects all <see cref="ListViewItem"/>s whose <see cref="ListViewItem.Tag"/> is contained within the list of <paramref name="tagValues"/>.
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="tagValues"></param>
        public static void SelectWithTag(this ListView listView, params object[] tagValues)
        {
            SelectWhere(listView, item => tagValues.Contains(item.Tag));
        }

        /// <summary>
        /// Selects all <see cref="ListViewItem"/>s for which the given <paramref name="condition"/> returns <c>true</c>.
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="condition"></param>
        public static void SelectWhere(this ListView listView, Func<ListViewItem, bool> condition)
        {
            listView.Items.OfType<ListViewItem>().ForEach(item => item.Selected = condition(item));
        }

        #region OS-specific extensions

#if __MonoCS__

        /// <summary>
        /// Does nothing on this platform.
        /// </summary>
        /// <param name="listViewControl"></param>
        /// <param name="columnIndex"></param>
        /// <param name="order"></param>
        /// <exception cref="Win32Exception"></exception>
        public static void SetSortIcon(this ListView listViewControl, int columnIndex, SortOrder order)
        {
        }

#else

        // ReSharper disable InconsistentNaming

        private const int LVM_FIRST = 0x1000;
        private const int LVM_GETHEADER = LVM_FIRST + 31;

        private const int HDM_FIRST = 0x1200;
        private const int HDM_GETITEM = HDM_FIRST + 11;
        private const int HDM_SETITEM = HDM_FIRST + 12;

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Displays a sort icon (up/down caret or chevron) on the given column.
        /// </summary>
        /// <param name="listViewControl"></param>
        /// <param name="columnIndex"></param>
        /// <param name="order"></param>
        /// <exception cref="Win32Exception"></exception>
        public static void SetSortIcon(this ListView listViewControl, int columnIndex, SortOrder order)
        {
            IntPtr columnHeader = WindowAPI.SendMessage(listViewControl.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
            for (int columnNumber = 0; columnNumber <= listViewControl.Columns.Count - 1; columnNumber++)
            {
                var columnPtr = new IntPtr(columnNumber);
                var item = new HDITEM
                {
                    mask = HeaderItemMask.HDI_FORMAT
                };

                if (WindowAPI.SendMessage(columnHeader, HDM_GETITEM, columnPtr, ref item) == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }

                if (order != SortOrder.None && columnNumber == columnIndex)
                {
                    switch (order)
                    {
                        case SortOrder.Ascending:
                            item.fmt &= ~HeaderItemFormat.HDF_SORTDOWN;
                            item.fmt |= HeaderItemFormat.HDF_SORTUP;
                            break;
                        case SortOrder.Descending:
                            item.fmt &= ~HeaderItemFormat.HDF_SORTUP;
                            item.fmt |= HeaderItemFormat.HDF_SORTDOWN;
                            break;
                    }
                }
                else
                {
                    item.fmt &= ~HeaderItemFormat.HDF_SORTDOWN & ~HeaderItemFormat.HDF_SORTUP;
                }

                if (WindowAPI.SendMessage(columnHeader, HDM_SETITEM, columnPtr, ref item) == IntPtr.Zero)
                {
                    throw new Win32Exception();
                }
            }
        }

#endif

        #endregion
    }
}