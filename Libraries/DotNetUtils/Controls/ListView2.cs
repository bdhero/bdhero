using System;
using System.Linq;
using System.Windows.Forms;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;

namespace DotNetUtils.Controls
{
    /// <summary>
    ///     A more sensible replacement for the default .NET <see cref="ListView"/> class.
    ///     Provides default sorting, column re-ordering, full row select, grid lines, tooltips,
    ///     defaults to detail view, disables multi-select, and displays row selection when the control loses focus.
    ///     Implements double buffering for smooth redrawing and auto-resizes columns when the control is resized.
    /// </summary>
    public class ListView2 : ListView
    {
        private readonly ListViewColumnSorter _columnSorter = new ListViewColumnSorter();

        /// <summary>
        ///     Constructs a new <see cref="ListView2"/> instance.
        /// </summary>
        public ListView2()
        {
            FullRowSelect = true;
            GridLines = true;
            View = View.Details;
            AllowColumnReorder = true;
            HideSelection = false;
            MultiSelect = false;
            ShowItemToolTips = true;

            this.SetDoubleBuffered(true);

            ListViewItemSorter = _columnSorter;
            ColumnClick += (_, e) => ToggleColumnSort(e.Column);

            KeyDown += OnKeyDown;
            DoubleClick += OnDoubleClick;

            var isResizing = false;

            // Automatically resize the last column to take up all remaining free space
            Resize += delegate
                      {
                          // listView.AutoSizeLastColumn() calls listView.ResumeDrawing(), which raises the Resize event.
                          // To prevent multiple recursive invocations of the Resize event, we make sure it's not already in progress.
                          if (isResizing) { return; }
                          isResizing = true;
                          this.AutoSizeLastColumn();
                          isResizing = false;
                      };
        }

        private ColumnHeader[] ColumnHeaders
        {
            get { return Columns.OfType<ColumnHeader>().ToArray(); }
        }

        private ColumnHeader GetColumnHeader(int index)
        {
            return ColumnHeaders[index];
        }

        /// <summary>
        ///     Gets the first column on the left based on <see cref="ColumnHeader.DisplayIndex" />.
        /// </summary>
        public ColumnHeader FirstDisplayedColumn
        {
            [CanBeNull]
            get
            {
                var columnHeaders = Columns.OfType<ColumnHeader>().ToArray();
                var minDisplayIndex = columnHeaders.Min(header => header.DisplayIndex);
                var firstColumn = columnHeaders.FirstOrDefault(header => header.DisplayIndex == minDisplayIndex);
                return firstColumn;
            }
        }

        /// <summary>
        ///     Gets the last column on the right based on <see cref="ColumnHeader.DisplayIndex" />.
        /// </summary>
        public ColumnHeader LastDisplayedColumn
        {
            [CanBeNull]
            get
            {
                var columnHeaders = Columns.OfType<ColumnHeader>().ToArray();
                var maxDisplayIndex = columnHeaders.Max(header => header.DisplayIndex);
                var lastColumn = columnHeaders.LastOrDefault(header => header.DisplayIndex == maxDisplayIndex);
                return lastColumn;
            }
        }

        /// <summary>
        ///     Toggles the sort direction of the column with the given <paramref name="columnIndex" />.
        /// </summary>
        /// <param name="columnIndex">Index of the column to sort.</param>
        public void ToggleColumnSort(int columnIndex)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (columnIndex == _columnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                _columnSorter.Order = _columnSorter.Order == SortOrder.Ascending
                                          ? SortOrder.Descending
                                          : SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _columnSorter.SortColumn = columnIndex;
                _columnSorter.Order = SortOrder.Ascending;

                var tag = GetColumnHeader(columnIndex).Tag;
                if (tag is SortOrder)
                {
                    _columnSorter.Order = (SortOrder) tag;
                }
            }

            // Perform the sort with these new sort options.
            Sort();

            this.SetSortIcon(_columnSorter.SortColumn, _columnSorter.Order);
        }

        #region Label editing

        private void EditSelectedListViewItem()
        {
            var items = SelectedItems.OfType<ListViewItem>().ToArray();
            if (!items.Any()) { return; }
            items.First().BeginEdit();
        }

        private void OnKeyDown(object sender, KeyEventArgs args)
        {
            if (!LabelEdit) { return; }
            if (args.KeyCode == Keys.F2)
            {
                EditSelectedListViewItem();
            }
        }

        private void OnDoubleClick(object sender, EventArgs eventArgs)
        {
            if (!LabelEdit) { return; }
            EditSelectedListViewItem();
        }

        #endregion
    }
}
