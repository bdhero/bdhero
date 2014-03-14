using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using NativeAPI.Win.UXTheme.SafeHandles;

namespace NativeAPI.Win.UXTheme
{
    public static class ThemeAPI
    {
        private static readonly ConcurrentDictionary<TextBoxBorderStyle, VisualStyleRenderer> Renderers
            = new ConcurrentDictionary<TextBoxBorderStyle, VisualStyleRenderer>();

        public static void DrawThemedTextBoxBorder(IntPtr hWnd, Graphics g, Rectangle bounds, TextBoxBorderStyle style = TextBoxBorderStyle.Normal)
        {
            // TODO: Hover fade effect?

//            DrawThemedTextBoxBorderManagedStandard(g, bounds, style);
//            DrawThemedTextBoxBorderManagedCustom(g, bounds, style);
//            DrawThemedTextBoxBorderNative(hWnd, g, bounds, style);

//            return;

            // TODO: Create a general purpose wrapper method for P/Invoke calls that handles exceptions
            // and optionally calls a backup/fallback method if it fails
            try
            {
                if (DrawThemedTextBoxBorderNative(hWnd, g, bounds, style))
                    return;
            }
            catch (DllNotFoundException e)
            {
                // Non-Windows OS
            }
            catch (EntryPointNotFoundException e)
            {
                // Older version of Windows (pre-XP)?
            }

            DrawThemedTextBoxBorderManaged(g, bounds, style);
        }

        private static void DrawThemedTextBoxBorderManaged(Graphics g, Rectangle bounds, TextBoxBorderStyle style)
        {
            if (DrawThemedTextBoxBorderManagedStandard(g, bounds, style))
                return;

            DrawThemedTextBoxBorderManagedCustom(g, bounds, style);
        }

        private static bool DrawThemedTextBoxBorderManagedStandard(Graphics g, Rectangle bounds, TextBoxBorderStyle style)
        {
            if (!VisualStyleRenderer.IsSupported)
                return false;

//            var renderer = Renderers.GetOrAdd(style, CreateVisualStyleRenderer);
            var renderer = CreateVisualStyleRenderer(style);
            if (renderer == null)
                return false;

            renderer.DrawBackground(g, bounds);

            return true;
        }

        private static VisualStyleRenderer CreateVisualStyleRenderer(TextBoxBorderStyle style)
        {
            var elem = style == TextBoxBorderStyle.Disabled ? VisualStyleElement.TextBox.TextEdit.Disabled :
                       style == TextBoxBorderStyle.ReadOnly ? VisualStyleElement.TextBox.TextEdit.ReadOnly :
                       style == TextBoxBorderStyle.Hot      ? VisualStyleElement.TextBox.TextEdit.Hot :
                       style == TextBoxBorderStyle.Focused  ? VisualStyleElement.TextBox.TextEdit.Focused :
                                                              VisualStyleElement.TextBox.TextEdit.Normal;

            if (!VisualStyleRenderer.IsElementDefined(elem))
                return null;

            return new VisualStyleRenderer(elem);
        }

        private static void DrawThemedTextBoxBorderManagedCustom(Graphics g, Rectangle bounds, TextBoxBorderStyle style)
        {
            // TODO: Get theme colors from somewhere in the Mono framework?
            var bgColor = Color.White;

            var borderWidth = 1;
            var borderColor = style == TextBoxBorderStyle.Disabled ? Color.LightGray :
                              style == TextBoxBorderStyle.Hot      ? Color.LightSlateGray :
                              style == TextBoxBorderStyle.Focused  ? Color.DimGray :
                                                                     Color.DarkGray;

            var size = bounds.Size;
            var topLeft = bounds.Location;
            var topRight = new Point(topLeft.X + size.Width - borderWidth, topLeft.Y);
            var bottomLeft = new Point(topLeft.X, topLeft.Y + size.Height - borderWidth);
            var bottomRight = new Point(topLeft.X + size.Width - borderWidth, topLeft.Y + size.Height - borderWidth);
            var points = new[]
                         {
                             topLeft,
                             topRight,
                             bottomRight,
                             bottomLeft,
                             topLeft,
                         };

            g.FillRectangle(new SolidBrush(bgColor), bounds);
            g.DrawLines(new Pen(borderColor, borderWidth), points);
        }

        private static bool DrawThemedTextBoxBorderNative(IntPtr hWnd, Graphics g, Rectangle bounds, TextBoxBorderStyle style)
        {
            if (!VisualStyleRenderer.IsSupported)
                return false;

            using (var themeData = CreateSafeThemeDataHandle(hWnd, VisualStyle.CLASS.EDIT))
            {
                if (themeData.IsInvalid)
                    return false;

                var part = VisualStyle.EDITPARTS.EP_EDITBORDER_NOSCROLL;
                var state = style == TextBoxBorderStyle.Disabled ? VisualStyle.EDITBORDER_NOSCROLLSTATES.EPSN_DISABLED :
                            style == TextBoxBorderStyle.Hot      ? VisualStyle.EDITBORDER_NOSCROLLSTATES.EPSN_HOT :
                            style == TextBoxBorderStyle.Focused  ? VisualStyle.EDITBORDER_NOSCROLLSTATES.EPSN_FOCUSED :
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
    }

    public enum TextBoxBorderStyle
    {
        Normal,
        ReadOnly,
        Disabled,
        Hot,
        Focused,
    }
}
