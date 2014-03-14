using System;
using Microsoft.Win32.SafeHandles;

namespace NativeAPI.Win.UXTheme.SafeHandles
{
    internal class SafeThemeDataHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public SafeThemeDataHandle(IntPtr themeDataPtr)
            : base(true)
        {
            handle = themeDataPtr;
        }

        protected override bool ReleaseHandle()
        {
            if (!CloseThemeData(handle))
                return false;

            handle = IntPtr.Zero;

            return true;
        }

        private static bool CloseThemeData(IntPtr handle)
        {
            return NativeMethods.CloseThemeData(handle) == WinErrorConstants.S_OK;
        }
    }
}
