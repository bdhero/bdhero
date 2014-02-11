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
using System.Runtime.InteropServices;
using System.Text;
using DotNetUtils.Annotations;
using OSUtils;
using OSUtils.JobObjects;

namespace WindowsOSUtils.JobObjects
{
    /// <summary>
    ///     Concrete implementation of the <see cref="IJobObjectManager"/> interface
    ///     that accesses the Windows Job Objects API.
    /// </summary>
    /// <seealso cref="https://www-auth.cs.wisc.edu/lists/htcondor-users/2009-June/msg00106.shtml" />
    public class JobObjectManager : IJobObjectManager
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     This is a pseudo handle to "any" job object.
        /// </summary>
        private static readonly IntPtr AnyJobHandle = IntPtr.Zero;

        public IJobObject CreateJobObject()
        {
            return new JobObject();
        }

        public bool IsAssignedToJob(Process process)
        {
            return HasJobObject(process);
        }

        public bool TryBypassPCA(string[] args)
        {
            Logger.Debug("Checking if current process belongs to a Job Object...");

            using (var currentProcess = Process.GetCurrentProcess())
            {
                if (!IsAssignedToJob(currentProcess))
                {
                    Logger.Debug("Current process does not belong to a Job Object.");
                    return false;
                }

                var fileName = currentProcess.MainModule.FileName;
                var arguments = new ArgumentList(args).ToString();

                Logger.InfoFormat("Spawning new child process outside of current job: \"{0}\" {1}", fileName, arguments);

                var startInfo = new ProcessStartInfo(fileName, arguments);
                var childProcess = CreateProcessInSeparateJob(startInfo);

                if (childProcess == null)
                {
                    Logger.Warn("Unable to spawn child process outside of current job.");
                    return false;
                }

                try
                {
                    var exited = childProcess.WaitForExit(2000);

                    if (exited)
                    {
                        Logger.Warn("Child process exited immediately after it was started.  " +
                                    "This may indicate a problem.");
                    }

                    return !exited;
                }
                catch (Exception e)
                {
                    Logger.Error("Failed to wait for child process to exit", e);
                    return false;
                }
            }
        }

        internal static bool HasJobObject(Process process)
        {
            return IsProcessInJob(process, AnyJobHandle);
        }

        /// <summary>
        ///     Determines if the given <paramref name="process"/> belongs to the specified
        ///     <paramref name="jobObjectHandle"/>.
        /// </summary>
        /// <param name="process">Process to check Job Object membership of.</param>
        /// <param name="jobObjectHandle">Job Object to check for membership.</param>
        /// <returns>
        ///     <c>true</c> if the given <paramref name="process"/> belongs to the specified
        ///     <paramref name="jobObjectHandle"/>; otherwise <c>false</c>.
        /// </returns>
        internal static bool IsProcessInJob(Process process, IntPtr jobObjectHandle)
        {
            var status = false;
            PInvokeUtils.Try(() => WinAPI.IsProcessInJob(process.Handle, jobObjectHandle, out status));
            return status;
        }

        /// <summary>
        ///     Starts a process outside of the current Job Object.
        ///     The child process will not inherit the current parent process' Job Object,
        ///     nor will it belong to any Job Object at all.
        /// </summary>
        /// <param name="startInfo">Process start information</param>
        /// <returns>
        ///     The child process if it was started successfully, or <c>null</c> if the process could not be started
        ///     (e.g., if the process is the Visual Studio <c>vhost.exe</c> application).
        /// </returns>
        /// <remarks>
        ///     This method can be used to break out of the Program Compatibility Assistant (PCA)
        ///     Job Object that is automatically created when an application runs on a newer
        ///     version of Windows than it was marked as compatible with in its application manifest.
        /// </remarks>
        /// <seealso cref="http://blogs.msdn.com/b/cjacks/archive/2009/07/10/how-to-work-around-program-compatibility-assistant-pca-jobobjects-interfering-with-your-jobobjects.aspx" />
        /// <seealso cref="http://blogs.msdn.com/b/alejacma/archive/2012/03/09/why-is-my-process-in-a-job-if-i-didn-t-put-it-there.aspx" />
        [CanBeNull]
        private static Process CreateProcessInSeparateJob(ProcessStartInfo startInfo)
        {
            var securityAttributes = new SECURITY_ATTRIBUTES
                                     {
                                         nLength = Marshal.SizeOf(typeof (SECURITY_ATTRIBUTES)),
                                         lpSecurityDescriptor = IntPtr.Zero,
                                         bInheritHandle = false
                                     };

            var environment = IntPtr.Zero;
            const bool inheritHandles = false;
            const string currentDirectory = null;
            const ProcessCreationFlags creationFlags = ProcessCreationFlags.CREATE_BREAKAWAY_FROM_JOB;

            var startupInfo = new STARTUPINFO { cb = Marshal.SizeOf(typeof (STARTUPINFO)) };
            var processInformation = new PROCESS_INFORMATION();

            PInvokeUtils.Try(() => WinAPI.CreateProcess(startInfo.FileName,
                                                        startInfo.Arguments,
                                                        ref securityAttributes,
                                                        ref securityAttributes,
                                                        inheritHandles,
                                                        creationFlags,
                                                        environment,
                                                        currentDirectory,
                                                        ref startupInfo,
                                                        out processInformation));

            try
            {
                return Process.GetProcessById(processInformation.dwProcessId);
            }
            catch (Exception e)
            {
                Logger.Error("Unable to get child process by ID.  " +
                             "Are you running in Visual Studio with Program Compatibility Assistant (PCA) enabled?  " +
                             "See http://stackoverflow.com/a/4232259/3205 for more information.", e);
                return null;
            }
        }
    }
}
