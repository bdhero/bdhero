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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NativeAPI.Win.Kernel;

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
                ThreadAPI.SuspendThread(ptr);
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
                ThreadAPI.ResumeThread(ptr);
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
            return ThreadAPI.OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)processThread.Id);
        }

        private static bool IsValidPointer(IntPtr ptr)
        {
            return ptr != IntPtr.Zero;
        }
    }
}
