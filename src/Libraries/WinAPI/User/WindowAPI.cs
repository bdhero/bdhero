// Copyright 2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Runtime.InteropServices;
using WinAPI.CommonControls;

namespace WinAPI.User
{
    public static class WindowAPI
    {
        /// <summary>
        ///     Retrieves information about the specified window.
        /// </summary>
        /// <param name="hwnd">
        ///     A handle to the window whose information is to be retrieved.
        /// </param>
        /// <param name="pwi">
        ///     A pointer to a <see cref="WINDOWINFO"/> structure to receive the information.
        ///     Note that you must set the <see cref="WINDOWINFO.cbSize"/> member to <c>sizeof(WINDOWINFO)</c>
        ///     <strong><em>before</em></strong> calling this function.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is nonzero.
        ///     If the function fails, the return value is zero.
        ///     To get extended error information, call <c>GetLastError</c>.
        /// </returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        #region ShowWindow

        /// <summary>
        ///     Sets the specified window's show state.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="nCmdShow">
        ///     Controls how the window is to be shown. This parameter is ignored the first time an application calls
        ///     <c>ShowWindow</c>, if the program that launched the application provides a <c>STARTUPINFO</c> structure.
        ///     Otherwise, the first time <c>ShowWindow</c> is called, the value should be the value obtained by the <c>WinMain</c>
        ///     function in its <paramref name="nCmdShow"/> parameter. In subsequent calls, this parameter can be one of
        ///     the <see cref="ShowWindowCommand"/> values.
        /// </param>
        /// <returns>
        ///     If the operation was successfully started, the return value is nonzero.
        /// </returns>
        /// <remarks>
        ///     This function posts a show-window event to the message queue of the given window.
        ///     An application can use this function to avoid becoming nonresponsive while waiting for a nonresponsive
        ///     application to finish processing a show-window event.
        /// </remarks>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, [MarshalAs(UnmanagedType.I4)] ShowWindowCommand nCmdShow);

        /// <summary>
        ///     Sets the show state of a window without waiting for the operation to complete.
        /// </summary>
        /// <param name="hWnd">
        ///     A handle to the window.
        /// </param>
        /// <param name="nCmdShow">
        ///     Controls how the window is to be shown. For a list of possible values, see the description of the <see cref="ShowWindow"/> function.
        /// </param>
        /// <returns>
        ///     If the operation was successfully started, the return value is nonzero.
        /// </returns>
        /// <remarks>
        ///     This function posts a show-window event to the message queue of the given window.
        ///     An application can use this function to avoid becoming nonresponsive while waiting for a nonresponsive
        ///     application to finish processing a show-window event.
        /// </remarks>
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, [MarshalAs(UnmanagedType.I4)] ShowWindowCommand nCmdShow);

        #endregion

        #region SendMessage

        /// <summary>
        ///     <para>
        ///         Sends the specified message to a window or windows. The SendMessage function calls the window
        ///         procedure for the specified window and does not return until the window procedure has processed the message.
        ///     </para>
        ///     <para>
        ///         To send a message and return immediately, use the SendMessageCallback or SendNotifyMessage function.
        ///         To post a message to a thread's message queue and return immediately, use the PostMessage or PostThreadMessage function.
        ///     </para>
        /// </summary>
        /// <param name="hWnd">
        ///     <para>
        ///         A handle to the window whose window procedure will receive the message. If this parameter is
        ///         HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system,
        ///         including disabled or invisible unowned windows, overlapped windows, and pop-up windows;
        ///         but the message is not sent to child windows.
        ///     </para>
        ///     <para>
        ///         Message sending is subject to UIPI. The thread of a process can send messages only to message
        ///         queues of threads in processes of lesser or equal integrity level.
        ///     </para>
        /// </param>
        /// <param name="wMsg">
        ///     <para>
        ///         The message to be sent.
        ///     </para>
        ///     <para>
        ///         For lists of the system-provided messages, see System-Defined Messages.
        ///     </para>
        /// </param>
        /// <param name="wParam">
        ///     Additional message-specific information.
        /// </param>
        /// <param name="lParam">
        ///     Additional message-specific information.
        /// </param>
        /// <returns>
        ///     The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         When a message is blocked by UIPI the last error, retrieved with GetLastError, is set to 5 (access denied).
        ///     </para>
        ///     <para>
        ///         Applications that need to communicate using HWND_BROADCAST should use the RegisterWindowMessage
        ///         function to obtain a unique message for inter-application communication.
        ///     </para>
        ///     <para>
        ///         The system only does marshalling for system messages (those in the range 0 to (WM_USER-1)).
        ///         To send other messages (those >= WM_USER) to another process, you must do custom marshalling.
        ///     </para>
        ///     <para>
        ///         If the specified window was created by the calling thread, the window procedure is called immediately
        ///         as a subroutine. If the specified window was created by a different thread, the system switches
        ///         to that thread and calls the appropriate window procedure. Messages sent between threads are
        ///         processed only when the receiving thread executes message retrieval code. The sending thread is
        ///         blocked until the receiving thread processes the message. However, the sending thread will process
        ///         incoming nonqueued messages while waiting for its message to be processed. To prevent this, use
        ///         SendMessageTimeout with SMTO_BLOCK set. For more information on nonqueued messages, see Nonqueued Messages.
        ///     </para>
        ///     <para>
        ///         An accessibility application can use SendMessage to send WM_APPCOMMAND messages to the shell to
        ///         launch applications. This functionality is not guaranteed to work for other types of applications.
        ///     </para>
        /// </remarks>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, [MarshalAs(UnmanagedType.U4)] WindowMessageType wMsg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        ///     <para>
        ///         Sends the specified message to a window or windows. The SendMessage function calls the window
        ///         procedure for the specified window and does not return until the window procedure has processed the message.
        ///     </para>
        ///     <para>
        ///         To send a message and return immediately, use the SendMessageCallback or SendNotifyMessage function.
        ///         To post a message to a thread's message queue and return immediately, use the PostMessage or PostThreadMessage function.
        ///     </para>
        /// </summary>
        /// <param name="hWnd">
        ///     <para>
        ///         A handle to the window whose window procedure will receive the message. If this parameter is
        ///         HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system,
        ///         including disabled or invisible unowned windows, overlapped windows, and pop-up windows;
        ///         but the message is not sent to child windows.
        ///     </para>
        ///     <para>
        ///         Message sending is subject to UIPI. The thread of a process can send messages only to message
        ///         queues of threads in processes of lesser or equal integrity level.
        ///     </para>
        /// </param>
        /// <param name="wMsg">
        ///     <para>
        ///         The message to be sent.
        ///     </para>
        ///     <para>
        ///         For lists of the system-provided messages, see System-Defined Messages.
        ///     </para>
        /// </param>
        /// <param name="wParam">
        ///     Additional message-specific information.
        /// </param>
        /// <param name="lParam">
        ///     Additional message-specific information.
        /// </param>
        /// <returns>
        ///     The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         When a message is blocked by UIPI the last error, retrieved with GetLastError, is set to 5 (access denied).
        ///     </para>
        ///     <para>
        ///         Applications that need to communicate using HWND_BROADCAST should use the RegisterWindowMessage
        ///         function to obtain a unique message for inter-application communication.
        ///     </para>
        ///     <para>
        ///         The system only does marshalling for system messages (those in the range 0 to (WM_USER-1)).
        ///         To send other messages (those >= WM_USER) to another process, you must do custom marshalling.
        ///     </para>
        ///     <para>
        ///         If the specified window was created by the calling thread, the window procedure is called immediately
        ///         as a subroutine. If the specified window was created by a different thread, the system switches
        ///         to that thread and calls the appropriate window procedure. Messages sent between threads are
        ///         processed only when the receiving thread executes message retrieval code. The sending thread is
        ///         blocked until the receiving thread processes the message. However, the sending thread will process
        ///         incoming nonqueued messages while waiting for its message to be processed. To prevent this, use
        ///         SendMessageTimeout with SMTO_BLOCK set. For more information on nonqueued messages, see Nonqueued Messages.
        ///     </para>
        ///     <para>
        ///         An accessibility application can use SendMessage to send WM_APPCOMMAND messages to the shell to
        ///         launch applications. This functionality is not guaranteed to work for other types of applications.
        ///     </para>
        /// </remarks>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, [MarshalAs(UnmanagedType.U4)] WindowMessageType wMsg, bool wParam, IntPtr lParam);

        /// <summary>
        ///     <para>
        ///         Sends the specified message to a window or windows. The SendMessage function calls the window
        ///         procedure for the specified window and does not return until the window procedure has processed the message.
        ///     </para>
        ///     <para>
        ///         To send a message and return immediately, use the SendMessageCallback or SendNotifyMessage function.
        ///         To post a message to a thread's message queue and return immediately, use the PostMessage or PostThreadMessage function.
        ///     </para>
        /// </summary>
        /// <param name="hWnd">
        ///     <para>
        ///         A handle to the window whose window procedure will receive the message. If this parameter is
        ///         HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system,
        ///         including disabled or invisible unowned windows, overlapped windows, and pop-up windows;
        ///         but the message is not sent to child windows.
        ///     </para>
        ///     <para>
        ///         Message sending is subject to UIPI. The thread of a process can send messages only to message
        ///         queues of threads in processes of lesser or equal integrity level.
        ///     </para>
        /// </param>
        /// <param name="wMsg">
        ///     <para>
        ///         The message to be sent.
        ///     </para>
        ///     <para>
        ///         For lists of the system-provided messages, see System-Defined Messages.
        ///     </para>
        /// </param>
        /// <param name="wParam">
        ///     Additional message-specific information.
        /// </param>
        /// <param name="lParam">
        ///     Additional message-specific information.
        /// </param>
        /// <returns>
        ///     The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         When a message is blocked by UIPI the last error, retrieved with GetLastError, is set to 5 (access denied).
        ///     </para>
        ///     <para>
        ///         Applications that need to communicate using HWND_BROADCAST should use the RegisterWindowMessage
        ///         function to obtain a unique message for inter-application communication.
        ///     </para>
        ///     <para>
        ///         The system only does marshalling for system messages (those in the range 0 to (WM_USER-1)).
        ///         To send other messages (those >= WM_USER) to another process, you must do custom marshalling.
        ///     </para>
        ///     <para>
        ///         If the specified window was created by the calling thread, the window procedure is called immediately
        ///         as a subroutine. If the specified window was created by a different thread, the system switches
        ///         to that thread and calls the appropriate window procedure. Messages sent between threads are
        ///         processed only when the receiving thread executes message retrieval code. The sending thread is
        ///         blocked until the receiving thread processes the message. However, the sending thread will process
        ///         incoming nonqueued messages while waiting for its message to be processed. To prevent this, use
        ///         SendMessageTimeout with SMTO_BLOCK set. For more information on nonqueued messages, see Nonqueued Messages.
        ///     </para>
        ///     <para>
        ///         An accessibility application can use SendMessage to send WM_APPCOMMAND messages to the shell to
        ///         launch applications. This functionality is not guaranteed to work for other types of applications.
        ///     </para>
        /// </remarks>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 wMsg, IntPtr wParam, ref HDITEM lParam);

        /// <summary>
        ///     <para>
        ///         Sends the specified message to a window or windows. The SendMessage function calls the window
        ///         procedure for the specified window and does not return until the window procedure has processed the message.
        ///     </para>
        ///     <para>
        ///         To send a message and return immediately, use the SendMessageCallback or SendNotifyMessage function.
        ///         To post a message to a thread's message queue and return immediately, use the PostMessage or PostThreadMessage function.
        ///     </para>
        /// </summary>
        /// <param name="hWnd">
        ///     <para>
        ///         A handle to the window whose window procedure will receive the message. If this parameter is
        ///         HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system,
        ///         including disabled or invisible unowned windows, overlapped windows, and pop-up windows;
        ///         but the message is not sent to child windows.
        ///     </para>
        ///     <para>
        ///         Message sending is subject to UIPI. The thread of a process can send messages only to message
        ///         queues of threads in processes of lesser or equal integrity level.
        ///     </para>
        /// </param>
        /// <param name="wMsg">
        ///     <para>
        ///         The message to be sent.
        ///     </para>
        ///     <para>
        ///         For lists of the system-provided messages, see System-Defined Messages.
        ///     </para>
        /// </param>
        /// <param name="wParam">
        ///     Additional message-specific information.
        /// </param>
        /// <param name="lParam">
        ///     Additional message-specific information.
        /// </param>
        /// <returns>
        ///     The return value specifies the result of the message processing; it depends on the message sent.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         When a message is blocked by UIPI the last error, retrieved with GetLastError, is set to 5 (access denied).
        ///     </para>
        ///     <para>
        ///         Applications that need to communicate using HWND_BROADCAST should use the RegisterWindowMessage
        ///         function to obtain a unique message for inter-application communication.
        ///     </para>
        ///     <para>
        ///         The system only does marshalling for system messages (those in the range 0 to (WM_USER-1)).
        ///         To send other messages (those >= WM_USER) to another process, you must do custom marshalling.
        ///     </para>
        ///     <para>
        ///         If the specified window was created by the calling thread, the window procedure is called immediately
        ///         as a subroutine. If the specified window was created by a different thread, the system switches
        ///         to that thread and calls the appropriate window procedure. Messages sent between threads are
        ///         processed only when the receiving thread executes message retrieval code. The sending thread is
        ///         blocked until the receiving thread processes the message. However, the sending thread will process
        ///         incoming nonqueued messages while waiting for its message to be processed. To prevent this, use
        ///         SendMessageTimeout with SMTO_BLOCK set. For more information on nonqueued messages, see Nonqueued Messages.
        ///     </para>
        ///     <para>
        ///         An accessibility application can use SendMessage to send WM_APPCOMMAND messages to the shell to
        ///         launch applications. This functionality is not guaranteed to work for other types of applications.
        ///     </para>
        /// </remarks>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 wMsg, IntPtr wParam, IntPtr lParam);

        #endregion
    }
}
