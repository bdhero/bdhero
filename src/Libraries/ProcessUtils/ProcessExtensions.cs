using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ProcessUtils
{
    /// <summary>
    /// Adds suspend and resume functionality to the <see cref="Process"/> class.
    /// </summary>
    public static class ProcessExtensions
    {
        /// <summary>
        /// Suspends the process by iterating over its threads and suspending each thread.
        /// </summary>
        /// <see cref="http://stackoverflow.com/questions/71257/suspend-process-in-c-sharp"/>
        public static void Suspend(this Process process)
        {
            if (process.HasExited || process.ProcessName == String.Empty)
                return;

            foreach (var ptr in process.GetThreadPointers())
            {
                SuspendThread(ptr);
            }
        }

        /// <summary>
        /// Resumes the process by iterating over its threads and resuming each thread.
        /// </summary>
        /// <see cref="http://stackoverflow.com/questions/71257/suspend-process-in-c-sharp"/>
        public static void Resume(this Process process)
        {
            if (process.HasExited || process.ProcessName == String.Empty)
                return;

            foreach (var ptr in process.GetThreadPointers())
            {
                ResumeThread(ptr);
            }
        }

        private static IEnumerable<IntPtr> GetThreadPointers(this Process process)
        {
            return process.Threads.Cast<ProcessThread>()
                          .Select(ThreadPointer)
                          .Where(IsValidPointer);
        }

        private static IntPtr ThreadPointer(ProcessThread processThread)
        {
            return OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)processThread.Id);
        }

        private static bool IsValidPointer(IntPtr ptr)
        {
            return ptr != IntPtr.Zero;
        }

        #region Win32

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local

        [Flags]
        private enum ThreadAccess
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        private static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        private static extern int ResumeThread(IntPtr hThread);

// ReSharper restore UnusedMember.Local
// ReSharper restore InconsistentNaming

        #endregion
    }
}
