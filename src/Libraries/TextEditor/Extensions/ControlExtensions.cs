using System.Windows.Forms;

namespace TextEditor.Extensions
{
    internal static class ControlExtensions
    {
        /// <summary>
        ///     Selects (focuses/activates) the next control forward or back in the tab order.
        /// </summary>
        /// <param name="startControl">
        ///     The <see cref="Control"/> to start the search with. 
        /// </param>
        /// <param name="forward">
        ///     <c>true</c> to search forward in the tab order; <c>false</c> to search backward. 
        /// </param>
        /// <returns>
        ///     <c>true</c> if a control was selected (activated); otherwise <c>false</c>.
        /// </returns>
        public static bool SelectNextControl(this Control startControl, bool forward)
        {
            var curControl = startControl;

            while (curControl != null && curControl.Parent != null)
            {
                var nextControl = curControl;

                // Skip the current and starting controls, as well as controls that cannot be selected or tabbed into.
                while (nextControl != null && (nextControl == curControl ||
                                               nextControl == startControl ||
                                               nextControl.CanSelect == false ||
                                               nextControl.TabStop == false))
                {
                    // Get the next control in the tab order from the current control's parent.
                    nextControl = curControl.Parent.GetNextControl(nextControl, forward);
                }

                if (nextControl != null)
                {
                    nextControl.Select();
                    nextControl.Focus();
                    return true;
                }

                curControl = curControl.Parent;
            }

            return false;
        }

        public static bool TabForward(this Control control)
        {
            return SelectNextControl(control, true);
        }

        public static bool TabBackward(this Control control)
        {
            return SelectNextControl(control, false);
        }
    }
}
