using System.Windows.Forms;

namespace DotNetUtils.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="TabPage"/> controls.
    /// </summary>
    public static class TabPageExtensions
    {
        /// <summary>
        /// Recursively enables or disables all child controls of the TabPage
        /// without enabling or disabling the TabPage itself.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="enable"></param>
        public static void EnableChildControls(this TabPage page, bool enable)
        {
            EnableControls(page.Controls, enable);
        }

        private static void EnableControls(Control.ControlCollection ctls, bool enable)
        {
            foreach (Control ctl in ctls)
            {
                ctl.Enabled = enable;
                EnableControls(ctl.Controls, enable);
            }
        }
    }
}