// Copyright 2012-2014 Andrew C. Dvorak
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

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
namespace NativeAPI.Win.Kernel
{
    /// <summary>
    ///     <para>
    ///         Contains information about a newly created process and its primary thread. It is used with the CreateProcess,
    ///         CreateProcessAsUser, CreateProcessWithLogonW, or CreateProcessWithTokenW function.
    ///     </para>
    /// </summary>
    /// <remarks>
    ///     If the function succeeds, be sure to call the CloseHandle function to close the hProcess and hThread handles when
    ///     you are finished with them. Otherwise, when the child process exits, the system cannot clean up the process
    ///     structures for the child process because the parent process still has open handles to the child process. However,
    ///     the system will close these handles when the parent process terminates, so the structures related to the child
    ///     process object would be cleaned up at this point.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESS_INFORMATION
    {
        /// <summary>
        ///     A handle to the newly created process. The handle is used to specify the process in all functions that perform
        ///     operations on the process object.
        /// </summary>
        public IntPtr hProcess;

        /// <summary>
        ///     A handle to the primary thread of the newly created process. The handle is used to specify the thread in all
        ///     functions that perform operations on the thread object.
        /// </summary>
        public IntPtr hThread;

        /// <summary>
        ///     A value that can be used to identify a process. The value is valid from the time the process is created until all
        ///     handles to the process are closed and the process object is freed; at this point, the identifier may be reused.
        /// </summary>
        public int dwProcessId;

        /// <summary>
        ///     A value that can be used to identify a thread. The value is valid from the time the thread is created until all
        ///     handles to the thread are closed and the thread object is freed; at this point, the identifier may be reused.
        /// </summary>
        public int dwThreadId;
    }
}