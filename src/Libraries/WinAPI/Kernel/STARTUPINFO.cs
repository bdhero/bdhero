using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
namespace WinAPI.Kernel
{
    /// <summary>
    ///     Specifies the window station, desktop, standard handles, and appearance of the main window for a process at
    ///     creation time.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct STARTUPINFO
    {
        /// <summary>
        ///     The size of the structure, in bytes.
        /// </summary>
        public Int32 cb;

        /// <summary>
        ///     Reserved; must be NULL.
        /// </summary>
        public string lpReserved;

        /// <summary>
        ///     The name of the desktop, or the name of both the desktop and window station for this process. A backslash in the
        ///     string indicates that the string includes both the desktop and window station names. For more information, see
        ///     Thread Connection to a Desktop.
        /// </summary>
        public string lpDesktop;

        /// <summary>
        ///     For console processes, this is the title displayed in the title bar if a new console window is created. If NULL,
        ///     the name of the executable file is used as the window title instead. This parameter must be NULL for GUI or console
        ///     processes that do not create a new console window.
        /// </summary>
        public string lpTitle;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USEPOSITION, this member is the x offset of the upper left corner of a window if
        ///         a new window is created, in pixels. Otherwise, this member is ignored.
        ///     </para>
        ///     <para>
        ///         The offset is from the upper left corner of the screen. For GUI processes, the specified position is used the
        ///         first time the new process calls CreateWindow to create an overlapped window if the x parameter of CreateWindow
        ///         is CW_USEDEFAULT.
        ///     </para>
        /// </summary>
        public Int32 dwX;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USEPOSITION, this member is the y offset of the upper left corner of a window if
        ///         a new window is created, in pixels. Otherwise, this member is ignored.
        ///     </para>
        ///     <para>
        ///         The offset is from the upper left corner of the screen. For GUI processes, the specified position is used the
        ///         first time the new process calls CreateWindow to create an overlapped window if the y parameter of CreateWindow
        ///         is CW_USEDEFAULT.
        ///     </para>
        /// </summary>
        public Int32 dwY;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USESIZE, this member is the width of the window if a new window is created, in
        ///         pixels. Otherwise, this member is ignored.
        ///     </para>
        ///     <para>
        ///         For GUI processes, this is used only the first time the new process calls CreateWindow to create an
        ///         overlapped window if the nWidth parameter of CreateWindow is CW_USEDEFAULT.
        ///     </para>
        /// </summary>
        public Int32 dwXSize;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USESIZE, this member is the height of the window if a new window is created, in
        ///         pixels. Otherwise, this member is ignored.
        ///     </para>
        ///     <para>
        ///         For GUI processes, this is used only the first time the new process calls CreateWindow to create an
        ///         overlapped window if the nHeight parameter of CreateWindow is CW_USEDEFAULT.
        ///     </para>
        /// </summary>
        public Int32 dwYSize;

        /// <summary>
        ///     If dwFlags specifies STARTF_USECOUNTCHARS, if a new console window is created in a console process, this member
        ///     specifies the screen buffer width, in character columns. Otherwise, this member is ignored.
        /// </summary>
        public Int32 dwXCountChars;

        /// <summary>
        ///     If dwFlags specifies STARTF_USECOUNTCHARS, if a new console window is created in a console process, this member
        ///     specifies the screen buffer height, in character rows. Otherwise, this member is ignored.
        /// </summary>
        public Int32 dwYCountChars;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USEFILLATTRIBUTE, this member is the initial text and background colors if a new
        ///         console window is created in a console application. Otherwise, this member is ignored.
        ///     </para>
        ///     <para>
        ///         This value can be any combination of the following values: FOREGROUND_BLUE, FOREGROUND_GREEN, FOREGROUND_RED,
        ///         FOREGROUND_INTENSITY, BACKGROUND_BLUE, BACKGROUND_GREEN, BACKGROUND_RED, and BACKGROUND_INTENSITY. For example,
        ///         the following combination of values produces red text on a white background:
        ///     </para>
        ///     <code>FOREGROUND_RED| BACKGROUND_RED| BACKGROUND_GREEN| BACKGROUND_BLUE</code>
        /// </summary>
        public Int32 dwFillAttribute;

        /// <summary>
        ///     A bitfield that determines whether certain STARTUPINFO members are used when the process creates a window.
        /// </summary>
        public STARTF dwFlags;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USESHOWWINDOW, this member can be any of the values that can be specified in the
        ///         nCmdShow parameter for the ShowWindow function, except for SW_SHOWDEFAULT. Otherwise, this member is ignored.
        ///     </para>
        ///     <para>
        ///         For GUI processes, the first time ShowWindow is called, its nCmdShow parameter is ignored wShowWindow
        ///         specifies the default value. In subsequent calls to ShowWindow, the wShowWindow member is used if the nCmdShow
        ///         parameter of ShowWindow is set to SW_SHOWDEFAULT.
        ///     </para>
        /// </summary>
        public Int16 wShowWindow;

        /// <summary>
        ///     Reserved for use by the C Run-time; must be zero.
        /// </summary>
        public Int16 cbReserved2;

        /// <summary>
        ///     Reserved for use by the C Run-time; must be NULL.
        /// </summary>
        public IntPtr lpReserved2;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USESTDHANDLES, this member is the standard input handle for the process. If
        ///         STARTF_USESTDHANDLES is not specified, the default for standard input is the keyboard buffer.
        ///     </para>
        ///     <para>
        ///         If dwFlags specifies STARTF_USEHOTKEY, this member specifies a hotkey value that is sent as the wParam
        ///         parameter of a WM_SETHOTKEY message to the first eligible top-level window created by the application that owns
        ///         the process. If the window is created with the WS_POPUP window style, it is not eligible unless the
        ///         WS_EX_APPWINDOW extended window style is also set. For more information, see CreateWindowEx.
        ///     </para>
        ///     <para>Otherwise, this member is ignored.</para>
        /// </summary>
        public IntPtr hStdInput;

        /// <summary>
        ///     <para>
        ///         If dwFlags specifies STARTF_USESTDHANDLES, this member is the standard output handle for the process.
        ///         Otherwise, this member is ignored and the default for standard output is the console window's buffer.
        ///     </para>
        ///     <para>
        ///         If a process is launched from the taskbar or jump list, the system sets hStdOutput to a handle to the monitor
        ///         that contains the taskbar or jump list used to launch the process. For more information, see Remarks.
        ///     </para>
        ///     <para>
        ///         Windows 7, Windows Server 2008 R2, Windows Vista, Windows Server 2008, Windows XP, and Windows Server 2003:
        ///         This behavior was introduced in Windows 8 and Windows Server 2012.
        ///     </para>
        /// </summary>
        public IntPtr hStdOutput;

        /// <summary>
        ///     If dwFlags specifies STARTF_USESTDHANDLES, this member is the standard error handle for the process. Otherwise,
        ///     this member is ignored and the default for standard error is the console window's buffer.
        /// </summary>
        public IntPtr hStdError;
    }
}