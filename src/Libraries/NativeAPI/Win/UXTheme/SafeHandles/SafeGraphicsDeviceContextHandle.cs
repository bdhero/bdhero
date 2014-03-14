using System;
using System.Drawing;
using Microsoft.Win32.SafeHandles;

namespace NativeAPI.Win.UXTheme.SafeHandles
{
    class SafeGraphicsDeviceContextHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private readonly Graphics _g;

        public SafeGraphicsDeviceContextHandle(Graphics g)
            : base(true)
        {
            _g = g;
            handle = g.GetHdc();
        }

        protected override bool ReleaseHandle()
        {
            _g.ReleaseHdc(handle);
            handle = IntPtr.Zero;
            return true;
        }
    }
}
