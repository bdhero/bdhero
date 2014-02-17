using System;
using System.Runtime.InteropServices;

namespace WinAPI.User
{
    /// <summary>
    ///     Windows API functions for interrogating and manipulating the cursor.
    /// </summary>
    public static class CursorAPI
    {
        /// <summary>
        ///     Loads the specified cursor resource from the executable (.EXE) file associated with an application instance.
        ///     <strong>Note</strong>: This function has been superseded by the <see cref="LoadImage"/> function.
        /// </summary>
        /// <param name="hInstance">
        ///     A handle to an instance of the module whose executable file contains the cursor to be loaded.
        /// </param>
        /// <param name="lpCursorName">
        ///     The name of the cursor resource to be loaded. Alternatively, this parameter can consist of the
        ///     resource identifier in the low-order word and zero in the high-order word.
        ///     To use one of the predefined cursors, the application must set the <paramref name="hInstance"/> parameter
        ///     to <c>NULL</c> and the <paramref name="lpCursorName"/> parameter to one of the <see cref="CursorType"/> values.
        /// </param>
        /// <returns>
        ///     <para>
        ///         If the function succeeds, the return value is the handle to the newly loaded cursor.
        ///     </para>
        ///     <para>
        ///         If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        ///     </para>
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The LoadCursor function loads the cursor resource only if it has not been loaded; otherwise,
        ///         it retrieves the handle to the existing resource. This function returns a valid cursor handle
        ///         only if the <paramref name="lpCursorName"/> parameter is a pointer to a cursor resource.
        ///         If <paramref name="lpCursorName"/> is a pointer to any type of resource other than a cursor
        ///         (such as an icon), the return value is not <c>NULL</c>, even though it is not a valid cursor handle.
        ///     </para>
        ///     <para>
        ///         The <c>LoadCursor</c> function searches the cursor resource most appropriate for the cursor for the
        ///         current display device. The cursor resource can be a color or monochrome bitmap.
        ///     </para>
        /// </remarks>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, [MarshalAs(UnmanagedType.I4)] CursorType lpCursorName);

        /// <summary>
        ///     Sets the cursor shape.
        /// </summary>
        /// <param name="hCursor">
        ///     A handle to the cursor. The cursor must have been created by the <see cref="CreateCursor"/> function or
        ///     loaded by the <see cref="LoadCursor"/> or <see cref="LoadImage"/> function. If this parameter is
        ///     <c>NULL</c>, the cursor is removed from the screen.
        /// </param>
        /// <returns>
        ///     <para>
        ///         The return value is the handle to the previous cursor, if there was one.
        ///     </para>
        ///     <para>
        ///         If there was no previous cursor, the return value is NULL.
        ///     </para>
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The cursor is set only if the new cursor is different from the previous cursor; otherwise, the function returns immediately.
        ///     </para>
        ///     <para>
        ///         The cursor is a shared resource. A window should set the cursor shape only when the cursor is in
        ///         its client area or when the window is capturing mouse input. In systems without a mouse, the window
        ///         should restore the previous cursor before the cursor leaves the client area or before it relinquishes
        ///         control to another window.
        ///     </para>
        ///     <para>
        ///         If your application must set the cursor while it is in a window, make sure the class cursor for the
        ///         specified window's class is set to <c>NULL</c>. If the class cursor is not <c>NULL</c>, the system restores the
        ///         class cursor each time the mouse is moved.
        ///     </para>
        ///     <para>
        ///         The cursor is not shown on the screen if the internal cursor display count is less than zero.
        ///         This occurs if the application uses the <see cref="ShowCursor"/> function to hide the cursor more times than to show the cursor.
        ///     </para>
        /// </remarks>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetCursor(IntPtr hCursor);
    }
}
