using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NativeAPI.Win.UXTheme
{
    internal static class NativeMethods
    {
        #region Constants

        public const int S_OK = 0;

        #endregion

        #region P/Invoke functions

        /// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/bb773210(v=vs.85).aspx"/>
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        public extern static int DrawThemeBackground(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, IntPtr pClipRect);

        /// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/bb759821(v=vs.85).aspx"/>
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr OpenThemeData(IntPtr hWnd, string classList);

        [DllImport("uxtheme.dll", ExactSpelling = true)]
        public extern static int CloseThemeData(IntPtr hTheme);

        #endregion

    }
}
