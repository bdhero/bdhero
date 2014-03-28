using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using NativeAPI.Win.UXTheme.SafeHandles;

namespace NativeAPI.Win.UXTheme
{
    public static class ThemeAPI
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     Paints an OS-native background and border on the given control.
        /// </summary>
        /// <param name="hWnd">
        ///     Handle to a Windows Forms control.
        /// </param>
        /// <param name="g">
        ///     Graphics object to paint.
        /// </param>
        /// <param name="bounds">
        ///     The bounds of the control.
        /// </param>
        /// <param name="state">
        ///     The desired style of the text box.
        /// </param>
        public static void DrawThemedTextBoxBorder(IntPtr hWnd, Graphics g, Rectangle bounds, TextBoxState state = TextBoxState.Normal)
        {
            // TODO: Hover fade effect?

            // TODO: Create a general purpose wrapper method for P/Invoke calls that handles exceptions
            // and optionally calls a backup/fallback method if it fails
            try
            {
                if (DrawThemedTextBoxBorderNative(hWnd, g, bounds, state))
                    return;
            }
            catch (DllNotFoundException ex)
            {
                // Non-Windows OS
                LogNativeException("Probably non-Windows OS?", ex);
            }
            catch (EntryPointNotFoundException ex)
            {
                // Older version of Windows (pre-XP)?
                LogNativeException("Probably older version of Windows (pre-XP)?", ex);
            }

            DrawThemedTextBoxBorderManaged(g, bounds, state);
        }

        private static void LogNativeException(string message, Exception ex)
        {
            var fullMessage = string.Format("DrawThemedTextBoxBorderNative() failed - {0} (OS = {1})",
                                            message, Environment.OSVersion);
            Logger.Info(fullMessage, ex);
        }

        #region Native Windows API

        private static bool DrawThemedTextBoxBorderNative(IntPtr hWnd, Graphics g, Rectangle bounds, TextBoxState style)
        {
            if (!VisualStyleRenderer.IsSupported)
                return false;

            using (var themeData = CreateSafeThemeDataHandle(hWnd, VisualStyle.CLASS.EDIT))
            {
                if (themeData.IsInvalid)
                    return false;

                var part = VisualStyle.EDITPARTS.EP_EDITBORDER_NOSCROLL;
                var state = style == TextBoxState.Disabled ? VisualStyle.EDITBORDER_NOSCROLLSTATES.EPSN_DISABLED :
                            style == TextBoxState.Hot      ? VisualStyle.EDITBORDER_NOSCROLLSTATES.EPSN_HOT :
                            style == TextBoxState.Focused  ? VisualStyle.EDITBORDER_NOSCROLLSTATES.EPSN_FOCUSED :
                                                             VisualStyle.EDITBORDER_NOSCROLLSTATES.EPSN_NORMAL;

                using (var graphicsDeviceContext = new SafeGraphicsDeviceContextHandle(g))
                {
                    DrawThemeBackground(themeData, graphicsDeviceContext, part, state, bounds);
                }
            }

            return true;
        }

        private static SafeThemeDataHandle CreateSafeThemeDataHandle(IntPtr hWnd, string className)
        {
            return new SafeThemeDataHandle(NativeMethods.OpenThemeData(hWnd, className));
        }

        private static int DrawThemeBackground(SafeThemeDataHandle themeDataHandle,
                                               SafeGraphicsDeviceContextHandle graphicsDeviceContextHandle,
                                               VisualStyle.EDITPARTS editPart,
                                               VisualStyle.EDITBORDER_NOSCROLLSTATES state,
                                               RECT area)
        {
            var hTheme = themeDataHandle.DangerousGetHandle();
            var hdc = graphicsDeviceContextHandle.DangerousGetHandle();

            var iPartId = (int) editPart;
            var iStateId = (int) state;

            return NativeMethods.DrawThemeBackground(hTheme, hdc, iPartId, iStateId, ref area, IntPtr.Zero);
        }

        #endregion

        #region Managed equivalent

        private class TextBoxTheme
        {
            public Color BorderColorDisabled;
            public Color BorderColorNormal;
            public Color BorderColorHot;
            public Color BorderColorFocused;
        }

        private static readonly TextBoxTheme Windows8Theme = new TextBoxTheme
                                                             {
                                                                 BorderColorDisabled = Color.FromArgb(0xd9, 0xd9, 0xd9),
                                                                 BorderColorNormal   = Color.FromArgb(0xab, 0xad, 0xb3),
                                                                 BorderColorHot      = Color.FromArgb(0x79, 0xb5, 0xed),
                                                                 BorderColorFocused  = Color.FromArgb(0x4d, 0x9e, 0xe9),
                                                             };

        private static void DrawThemedTextBoxBorderManaged(Graphics g, Rectangle bounds, TextBoxState state)
        {
            var theme = Windows8Theme;

            var color = (state == TextBoxState.Disabled) ? theme.BorderColorDisabled :
                        (state == TextBoxState.Hot)      ? theme.BorderColorHot :
                        (state == TextBoxState.Focused)  ? theme.BorderColorFocused :
                                                           theme.BorderColorNormal;

            ControlPaint.DrawBorder(g, bounds, color, ButtonBorderStyle.Solid);
        }

        #endregion
    }
}
