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

namespace NativeAPI.Win.Kernel
{
    /// <summary>
    ///     Limit flags that are in effect. This member is a bit field that determines
    ///     whether other structure members are used. Any combination of the following
    ///     values can be specified.
    /// </summary>
    [Flags]
    public enum LimitFlags
    {
        /// <summary>
        ///     Causes all processes associated with the job to use the same minimum and maximum working set sizes.
        /// </summary>
        LimitWorkingSet = 0x00000001,

        /// <summary>
        ///     Establishes a user-mode execution time limit for each currently active process
        ///     and for all future processes associated with the job.
        /// </summary>
        LimitProcessTime = 0x00000002,

        /// <summary>
        ///     Establishes a user-mode execution time limit for the job. This flag cannot
        ///     be used with JOB_OBJECT_LIMIT_PRESERVE_JOB_TIME.
        /// </summary>
        LimitJobTime = 0x00000004,

        /// <summary>
        ///     Establishes a maximum number of simultaneously active processes associated
        ///     with the job.
        /// </summary>
        LimitActiveProcesses = 0x00000008,

        /// <summary>
        ///     Causes all processes associated with the job to use the same processor
        ///     affinity.
        /// </summary>
        LimitAffinity = 0x00000010,

        /// <summary>
        ///     Causes all processes associated with the job to use the same priority class.
        ///     For more information, see Scheduling Priorities.
        /// </summary>
        LimitPriorityClass = 0x00000020,

        /// <summary>
        ///     Preserves any job time limits you previously set. As long as this flag is
        ///     set, you can establish a per-job time limit once, then alter other limits
        ///     in subsequent calls. This flag cannot be used with JOB_OBJECT_LIMIT_JOB_TIME.
        /// </summary>
        PreserveJobTime = 0x00000040,

        /// <summary>
        ///     Causes all processes in the job to use the same scheduling class.
        /// </summary>
        LimitSchedulingClass = 0x00000080,

        /// <summary>
        ///     Causes all processes associated with the job to limit their committed memory.
        ///     When a process attempts to commit memory that would exceed the per-process
        ///     limit, it fails. If the job object is associated with a completion port, a
        ///     JOB_OBJECT_MSG_PROCESS_MEMORY_LIMIT message is sent to the completion port.
        ///     This limit requires use of a JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure.
        ///     Its BasicLimitInformation member is a JOBOBJECT_BASIC_LIMIT_INFORMATION
        ///     structure.
        /// </summary>
        LimitProcessMemory = 0x00000100,

        /// <summary>
        ///     Causes all processes associated with the job to limit the job-wide sum of
        ///     their committed memory. When a process attempts to commit memory that would
        ///     exceed the job-wide limit, it fails. If the job object is associated with a
        ///     completion port, a JOB_OBJECT_MSG_JOB_MEMORY_LIMIT message is sent to the
        ///     completion port.  This limit requires use of a
        ///     JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure. Its BasicLimitInformation
        ///     member is a JOBOBJECT_BASIC_LIMIT_INFORMATION structure.
        /// </summary>
        LimitJobMemory = 0x00000200,

        /// <summary>
        ///     Forces a call to the SetErrorMode function with the SEM_NOGPFAULTERRORBOX
        ///     flag for each process associated with the job.  If an exception occurs and
        ///     the system calls the UnhandledExceptionFilter function, the debugger will
        ///     be given a chance to act. If there is no debugger, the functions returns
        ///     EXCEPTION_EXECUTE_HANDLER. Normally, this will cause termination of the
        ///     process with the exception code as the exit status.  This limit requires
        ///     use of a JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure. Its
        ///     BasicLimitInformation member is a JOBOBJECT_BASIC_LIMIT_INFORMATION structure.
        /// </summary>
        DieOnUnhandledException = 0x00000400,

        /// <summary>
        ///     If any process associated with the job creates a child process using the
        ///     CREATE_BREAKAWAY_FROM_JOB flag while this limit is in effect, the child
        ///     process is not associated with the job.  This limit requires use of a
        ///     JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure. Its BasicLimitInformation
        ///     member is a JOBOBJECT_BASIC_LIMIT_INFORMATION structure.
        /// </summary>
        LimitBreakawayOk = 0x00000800,

        /// <summary>
        ///     Allows any process associated with the job to create child processes
        ///     that are not associated with the job.  This limit requires use of a
        ///     JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure. Its BasicLimitInformation
        ///     member is a JOBOBJECT_BASIC_LIMIT_INFORMATION structure.
        /// </summary>
        LimitSilentBreakawayOk = 0x00001000,

        /// <summary>
        ///     Causes all processes associated with the job to terminate when the last
        ///     handle to the job is closed.  This limit requires use of a
        ///     JOBOBJECT_EXTENDED_LIMIT_INFORMATION structure. Its BasicLimitInformation
        ///     member is a JOBOBJECT_BASIC_LIMIT_INFORMATION structure.
        ///     Windows 2000:  This flag is not supported.
        /// </summary>
        LimitKillOnJobClose = 0x00002000
    }
}