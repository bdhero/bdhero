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

namespace NativeAPI.Win.Kernel
{
    /// <summary>
    ///     Windows API functions for interrogating and manipulating process threads.
    /// </summary>
    public static class ThreadAPI
    {
        /// <summary>
        ///     Opens an existing thread object.
        /// </summary>
        /// <param name="dwDesiredAccess">
        ///     <para>
        ///         The access to the thread object. This access right is checked against the security descriptor for
        ///         the thread. This parameter can be one or more of the thread access rights.
        ///     </para>
        ///     <para>
        ///         If the caller has enabled the SeDebugPrivilege privilege, the requested access is granted
        ///         regardless of the contents of the security descriptor.
        ///     </para>
        /// </param>
        /// <param name="bInheritHandle">
        ///     If this value is TRUE, processes created by this process will inherit the handle. Otherwise, the processes do not inherit this handle.
        /// </param>
        /// <param name="dwThreadId">
        ///     The identifier of the thread to be opened.
        /// </param>
        /// <returns>
        ///     <para>If the function succeeds, the return value is an open handle to the specified thread.</para>
        ///     <para>If the function fails, the return value is NULL. To get extended error information, call GetLastError.</para>
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The handle returned by OpenThread can be used in any function that requires a handle to a thread,
        ///         such as the wait functions, provided you requested the appropriate access rights. The handle is
        ///         granted access to the thread object only to the extent it was specified in the <paramref name="dwDesiredAccess"/> parameter.
        ///     </para>
        ///     <para>
        ///         When you are finished with the handle, be sure to close it by using the <see cref="CloseHandle"/> function.
        ///     </para>
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        /// <summary>
        ///     Suspends the specified thread.
        ///     A 64-bit application can suspend a WOW64 thread using the <see cref="Wow64SuspendThread"/> function.
        /// </summary>
        /// <param name="hThread">
        ///     <para>
        ///         A handle to the thread that is to be suspended.
        ///     </para>
        ///     <para>
        ///         The handle must have the THREAD_SUSPEND_RESUME access right. For more information, see Thread Security and Access Rights.
        ///     </para>
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is the thread's previous suspend count;
        ///     otherwise, it is (DWORD) -1. To get extended error information, use the GetLastError function.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If the function succeeds, execution of the specified thread is suspended and the thread's suspend count
        ///         is incremented. Suspending a thread causes the thread to stop executing user-mode (application) code.
        ///     </para>
        ///     <para>
        ///         This function is primarily designed for use by debuggers. It is not intended to be used for thread
        ///         synchronization. Calling <see cref="SuspendThread"/> on a thread that owns a synchronization object, such as a
        ///         mutex or critical section, can lead to a deadlock if the calling thread tries to obtain a
        ///         synchronization object owned by a suspended thread. To avoid this situation, a thread within an
        ///         application that is not a debugger should signal the other thread to suspend itself. The target
        ///         thread must be designed to watch for this signal and respond appropriately.
        ///     </para>
        ///     <para>
        ///         Each thread has a suspend count (with a maximum value of MAXIMUM_SUSPEND_COUNT). If the suspend
        ///         count is greater than zero, the thread is suspended; otherwise, the thread is not suspended and
        ///         is eligible for execution. Calling <see cref="SuspendThread"/> causes the target thread's suspend count to be
        ///         incremented. Attempting to increment past the maximum suspend count causes an error without incrementing the count.
        ///     </para>
        ///     <para>
        ///         The <see cref="ResumeThread"/> function decrements the suspend count of a suspended thread.
        ///     </para>
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint SuspendThread(IntPtr hThread);

        /// <summary>
        ///     Suspends the specified WOW64 thread.
        /// </summary>
        /// <param name="hThread">
        ///     <para>
        ///         A handle to the thread that is to be suspended.
        ///     </para>
        ///     <para>
        ///         The handle must have the THREAD_SUSPEND_RESUME access right. For more information, see Thread Security and Access Rights.
        ///     </para>
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is the thread's previous suspend count;
        ///     otherwise, it is (DWORD) -1. To get extended error information, use the GetLastError function.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If the function succeeds, execution of the specified thread is suspended and the thread's
        ///         suspend count is incremented. Suspending a thread causes the thread to stop executing user-mode (application) code.
        ///     </para>
        ///     <para>
        ///         This function is primarily designed for use by debuggers. It is not intended to be used for
        ///         thread synchronization. Calling <see cref="Wow64SuspendThread"/> on a thread that owns a synchronization object,
        ///         such as a mutex or critical section, can lead to a deadlock if the calling thread tries to obtain
        ///         a synchronization object owned by a suspended thread. To avoid this situation, a thread within
        ///         an application that is not a debugger should signal the other thread to suspend itself.
        ///         The target thread must be designed to watch for this signal and respond appropriately.
        ///     </para>
        ///     <para>
        ///         Each thread has a suspend count (with a maximum value of MAXIMUM_SUSPEND_COUNT). If the
        ///         suspend count is greater than zero, the thread is suspended; otherwise, the thread is not
        ///         suspended and is eligible for execution. Calling <see cref="Wow64SuspendThread"/> causes the target thread's
        ///         suspend count to be incremented. Attempting to increment past the maximum suspend count causes
        ///         an error without incrementing the count.
        ///     </para>
        ///     <para>
        ///         The <see cref="ResumeThread"/> function decrements the suspend count of a suspended thread.
        ///     </para>
        ///     <para>
        ///         This function is intended for 64-bit applications. It is not supported on 32-bit Windows;
        ///         such calls fail and set the last error code to ERROR_INVALID_FUNCTION. A 32-bit application
        ///         can call this function on a WOW64 thread; the result is the same as calling the <see cref="SuspendThread"/> function.
        ///     </para>
        /// </remarks>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint Wow64SuspendThread(IntPtr hThread);

        /// <summary>
        ///     Decrements a thread's suspend count. When the suspend count is decremented to zero, the execution of the thread is resumed.
        /// </summary>
        /// <param name="hThread">
        ///     <para>
        ///         A handle to the thread to be restarted.
        ///     </para>
        ///     <para>
        ///         This handle must have the THREAD_SUSPEND_RESUME access right. For more information, see Thread Security and Access Rights.
        ///     </para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         If the function succeeds, the return value is the thread's previous suspend count.
        ///     </para>
        ///     <para>
        ///         If the function fails, the return value is (DWORD) -1. To get extended error information, call GetLastError.
        ///     </para>
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The <see cref="ResumeThread"/> function checks the suspend count of the subject thread. If the suspend count is zero,
        ///         the thread is not currently suspended. Otherwise, the subject thread's suspend count is decremented.
        ///         If the resulting value is zero, then the execution of the subject thread is resumed.
        ///     </para>
        ///     <para>
        ///         If the return value is zero, the specified thread was not suspended. If the return value is 1,
        ///         the specified thread was suspended but was restarted. If the return value is greater than 1,
        ///         the specified thread is still suspended.
        ///     </para>
        ///     <para>
        ///         Note that while reporting debug events, all threads within the reporting process are frozen.
        ///         Debuggers are expected to use the <see cref="SuspendThread"/> and <see cref="ResumeThread"/> functions to limit the set of
        ///         threads that can execute within a process. By suspending all threads in a process except for the
        ///         one reporting a debug event, it is possible to "single step" a single thread. The other threads
        ///         are not released by a continue operation if they are suspended.
        ///     </para>
        /// </remarks>
        [DllImport("kernel32.dll")]
        public static extern int ResumeThread(IntPtr hThread);
    }
}
