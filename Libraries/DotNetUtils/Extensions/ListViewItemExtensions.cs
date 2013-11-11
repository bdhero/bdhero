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