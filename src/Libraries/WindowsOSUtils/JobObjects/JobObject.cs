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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using WindowsOSUtils.WinAPI.Kernel;
using OSUtils.JobObjects;

namespace WindowsOSUtils.JobObjects
{
    /// <summary>
    ///     Represents a Windows Job Object that groups two or more processes together.
    /// </summary>
    /// <remarks>
    ///     A job object allows groups of processes to be managed as a unit.
    ///     Job objects are namable, securable, sharable objects that control attributes
    ///     of the processes associated with them. Operations performed on a job object
    ///     affect all processes associated with the job object. Examples include
    ///     enforcing limits such as working set size and process priority or terminating
    ///     all processes associated with a job.
    /// </remarks>
    /// <see cref="http://skolima.blogspot.com/2012/09/handling-native-api-in-managed.html" />
    public sealed class JobObject : IJobObject
    {
        private readonly IntPtr _jobObjectHandle;

        private bool _disposed;

        /// <exception cref="Win32Exception">
        ///     Thrown if the operating system was unable to create a new Job Object.
        /// </exception>
        public JobObject()
        {
            _jobObjectHandle = PInvokeUtils.Try(() => JobObjectAPI.CreateJobObject(IntPtr.Zero, null));
        }

        #region IDisposable Members

        ~JobObject()
        {
            Dispose(false); // I am *not* calling you from Dispose, it's *not* safe to free managed resources
        }

        /// <summary>
        ///     Frees managed and unmanaged resources.
        /// </summary>
        /// <param name="freeManagedObjectsAlso">
        ///     Free managed resources.  Should only be set to <c>true</c> when called from <see cref="Dispose"/>.
        /// </param>
        /// <exception cref="Win32Exception">
        ///     Thrown if the handle to the Job Object could not be released.
        /// </exception>
        /// <seealso cref="http://stackoverflow.com/a/538238/467582"/>
        private void Dispose(bool freeManagedObjectsAlso)
        {
            // Free unmanaged resources
            // ...

            // Free managed resources too, but only if I'm being called from Dispose()
            // (If I'm being called from Finalize then the objects might not exist anymore)
            if (freeManagedObjectsAlso)
            {
                if (_disposed) { return; }
                if (_jobObjectHandle == IntPtr.Zero) { return; }

                _disposed = true;

                PInvokeUtils.Try(() => JobObjectAPI.CloseHandle(_jobObjectHandle));
            }
        }

        /// <exception cref="Win32Exception">
        ///     Thrown if the handle to the Job Object could not be released.
        /// </exception>
        /// <seealso cref="http://stackoverflow.com/a/538238/467582"/>
        public void Dispose()
        {
            Dispose(true); // I am calling you from Dispose, it's safe
            GC.SuppressFinalize(this); // Hey, GC: don't bother calling finalize later
        }

        #endregion

        /// <summary>
        ///     Assigns the given process to this Job Object.
        /// </summary>
        /// <param name="process">Process to assign to this Job Object.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="process"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if <paramref name="process"/> already belongs to a Job Object.
        /// </exception>
        /// <exception cref="Win32Exception">
        ///     Thrown if the operating system was unable to assign <paramref name="process"/> to the Job Object.
        /// </exception>
        public void Assign(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (AlreadyAssigned(process))
                return;

            if (HasJobObject(process))
                throw new InvalidOperationException(
                    "Requested process already belongs to another job group.  Check http://stackoverflow.com/a/4232259/3205 for help.");

            PInvokeUtils.Try(() => JobObjectAPI.AssignProcessToJobObject(_jobObjectHandle, process.Handle));
        }

        public void KillOnClose()
        {
            var type = JobObjectInfoClass.ExtendedLimitInformation;
            var limit = CreateKillOnCloseJobObjectInfo();
            var length = GetKillOnCloseJobObjectInfoLength();

            PInvokeUtils.Try(() => JobObjectAPI.SetInformationJobObject(_jobObjectHandle, type, ref limit, length));
        }

        private static uint GetKillOnCloseJobObjectInfoLength()
        {
            var type = PInvokeUtils.Is32Bit
                           ? typeof (ExtendedLimits32)
                           : typeof (ExtendedLimits64);
            return (uint) Marshal.SizeOf(type);
        }

        private static JobObjectInfo CreateKillOnCloseJobObjectInfo()
        {
            return PInvokeUtils.Is32Bit
                       ? CreateKillOnCloseJobObjectInfo32()
                       : CreateKillOnCloseJobObjectInfo64();
        }

        private static JobObjectInfo CreateKillOnCloseJobObjectInfo32()
        {
            return new JobObjectInfo
            {
                extendedLimits32 = new ExtendedLimits32
                {
                    BasicLimits = new BasicLimits32
                    {
                        LimitFlags = LimitFlags.LimitKillOnJobClose
                    }
                }
            };
        }

        private static JobObjectInfo CreateKillOnCloseJobObjectInfo64()
        {
            return new JobObjectInfo
            {
                extendedLimits64 = new ExtendedLimits64
                {
                    BasicLimits = new BasicLimits64
                    {
                        LimitFlags = LimitFlags.LimitKillOnJobClose
                    }
                }
            };
        }

        /// <summary>
        ///     Determines if the given <paramref name="process"/> already belongs to <b>this</b> Job Object.
        /// </summary>
        /// <param name="process">Process to check Job Object membership of.</param>
        /// <returns>
        ///     <c>true</c> if the given <paramref name="process"/> already belongs to this Job Object;
        ///     otherwise <c>false</c>.
        /// </returns>
        private bool AlreadyAssigned(Process process)
        {
            return JobObjectManager.IsProcessInJob(process, _jobObjectHandle);
        }

        /// <summary>
        ///     Determines if the given <paramref name="process"/> already belongs to <b>another</b> Job Object.
        /// </summary>
        /// <param name="process">Process to check Job Object membership of.</param>
        /// <returns>
        ///     <c>true</c> if the given <paramref name="process"/> already belongs to another Job Object;
        ///     otherwise <c>false</c>.
        /// </returns>
        private static bool HasJobObject(Process process)
        {
            return JobObjectManager.HasJobObject(process);
        }
    }
}
