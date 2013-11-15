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