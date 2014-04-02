using System.Linq;
using System.Windows.Forms;
using UILib.Extensions;

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
            var nextControl = startControl.GetNextControl(forward);
            if (nextControl == null)
                return false;

            nextControl.Select();
            nextControl.Focus();

            return true;
        }

        /// <summary>
        ///     Gets the next control forward or back in the tab order.
        /// </summary>
        /// <param name="startControl">
        ///     The <see cref="Control"/> to start the search with. 
        /// </param>
        /// <param name="forward">
        ///     <c>true</c> to search forward in the tab order; <c>false</c> to search backward. 
        /// </param>
        /// <returns>
        ///     The next control forward or back in the tab order (depending on the value of <paramref name="forward"/>),
        ///     or <c>null</c> if no next control was found.
        /// </returns>
        public static Control GetNextControl(this Control startControl, bool forward)
        {
            var nextControl = GetNextControlImpl(startControl, forward);

            // Wrap around if there are no more controls in the requested direction.
            // We only need this extra check if we're going backward.
            // It doesn't appear to be necessary when going forward;
            // WinForms seems to handle foward wrapping automatically.
            if (nextControl == null && !forward)
            {
                var curControl = startControl;

                while ((curControl = curControl.GetNextControlImpl(!forward)) != null &&
                       curControl != startControl &&
                       !startControl.Descendants().Contains(curControl) &&
                       !curControl.Descendants().Contains(startControl))
                {
                    nextControl = curControl;
                }
            }

            return nextControl;
        }

        private static Control GetNextControlImpl(this Control startControl, bool forward)
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
                    return nextControl;
                }

                curControl = curControl.Parent;
            }

            return null;
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
