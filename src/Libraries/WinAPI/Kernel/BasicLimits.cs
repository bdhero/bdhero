using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
namespace WinAPI.Kernel
{
    #region 32-bit

    /// <summary>
    ///     The JOBOBJECT_BASIC_LIMIT_INFORMATION structure contains basic limit
    ///     information for a job object.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct BasicLimits32
    {
        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_PROCESS_TIME, this member is
        ///     the per-process user-mode execution time limit, in 100-nanosecond ticks.
        ///     Otherwise, this member is ignored.  The system periodically checks to
        ///     determine whether each process associated with the job has accumulated
        ///     more user-mode time than the set limit. If it has, the process is terminated.
        /// </summary>
        [FieldOffset(0)]
        public long PerProcessUserTimeLimit;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_JOB_TIME, this member is the
        ///     per-job user-mode execution time limit, in 100-nanosecond ticks. Otherwise,
        ///     this member is ignored. The system adds the current time of the processes
        ///     associated with the job to this limit. For example, if you set this limit
        ///     to 1 minute, and the job has a process that has accumulated 5 minutes of
        ///     user-mode time, the limit actually enforced is 6 minutes.  The system
        ///     periodically checks to determine whether the sum of the user-mode execution
        ///     time for all processes is greater than this end-of-job limit. If it is, the
        ///     action specified in the EndOfJobTimeAction member of the
        ///     JOBOBJECT_END_OF_JOB_TIME_INFORMATION structure is carried out. By default,
        ///     all processes are terminated and the status code is set to
        ///     ERROR_NOT_ENOUGH_QUOTA.
        /// </summary>
        [FieldOffset(8)]
        public long PerJobUserTimeLimit;

        /// <summary>
        ///     Limit flags that are in effect. This member is a bit field that determines
        ///     whether other structure members are used. Any combination LimitFlag values
        ///     can be specified.
        /// </summary>
        [FieldOffset(16)]
        public LimitFlags LimitFlags;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_WORKINGSET, this member is the
        ///     minimum working set size for each process associated with the job. Otherwise,
        ///     this member is ignored.  If MaximumWorkingSetSize is nonzero,
        ///     MinimumWorkingSetSize cannot be zero.
        /// </summary>
        [FieldOffset(20)]
        public uint MinimumWorkingSetSize;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_WORKINGSET, this member is the
        ///     maximum working set size for each process associated with the job. Otherwise,
        ///     this member is ignored.  If MinimumWorkingSetSize is nonzero,
        ///     MaximumWorkingSetSize cannot be zero.
        /// </summary>
        [FieldOffset(24)]
        public uint MaximumWorkingSetSize;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_ACTIVE_PROCESS, this member is the
        ///     active process limit for the job. Otherwise, this member is ignored.  If you
        ///     try to associate a process with a job, and this causes the active process
        ///     count to exceed this limit, the process is terminated and the association
        ///     fails.
        /// </summary>
        [FieldOffset(28)]
        public int ActiveProcessLimit;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_AFFINITY, this member is the
        ///     processor affinity for all processes associated with the job. Otherwise,
        ///     this member is ignored.  The affinity must be a proper subset of the system
        ///     affinity mask obtained by calling the GetProcessAffinityMask function. The
        ///     affinity of each thread is set to this value, but threads are free to
        ///     subsequently set their affinity, as long as it is a subset of the specified
        ///     affinity mask. Processes cannot set their own affinity mask.
        /// </summary>
        [FieldOffset(32)]
        public IntPtr Affinity;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_PRIORITY_CLASS, this member is the
        ///     priority class for all processes associated with the job. Otherwise, this
        ///     member is ignored. Processes and threads cannot modify their priority class.
        ///     The calling process must enable the SE_INC_BASE_PRIORITY_NAME privilege.
        /// </summary>
        [FieldOffset(36)]
        public int PriorityClass;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_SCHEDULING_CLASS, this member is
        ///     the scheduling class for all processes associated with the job. Otherwise,
        ///     this member is ignored.  The valid values are 0 to 9. Use 0 for the least
        ///     favorable scheduling class relative to other threads, and 9 for the most
        ///     favorable scheduling class relative to other threads. By default, this
        ///     value is 5. To use a scheduling class greater than 5, the calling process
        ///     must enable the SE_INC_BASE_PRIORITY_NAME privilege.
        /// </summary>
        [FieldOffset(40)]
        public int SchedulingClass;
    }

    #endregion

    #region 64-bit

    /// <summary>
    ///     The JOBOBJECT_BASIC_LIMIT_INFORMATION structure contains basic limit
    ///     information for a job object.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct BasicLimits64
    {
        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_PROCESS_TIME, this member is
        ///     the per-process user-mode execution time limit, in 100-nanosecond ticks.
        ///     Otherwise, this member is ignored.  The system periodically checks to
        ///     determine whether each process associated with the job has accumulated
        ///     more user-mode time than the set limit. If it has, the process is terminated.
        /// </summary>
        [FieldOffset(0)]
        public long PerProcessUserTimeLimit;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_JOB_TIME, this member is the
        ///     per-job user-mode execution time limit, in 100-nanosecond ticks. Otherwise,
        ///     this member is ignored. The system adds the current time of the processes
        ///     associated with the job to this limit. For example, if you set this limit
        ///     to 1 minute, and the job has a process that has accumulated 5 minutes of
        ///     user-mode time, the limit actually enforced is 6 minutes.  The system
        ///     periodically checks to determine whether the sum of the user-mode execution
        ///     time for all processes is greater than this end-of-job limit. If it is, the
        ///     action specified in the EndOfJobTimeAction member of the
        ///     JOBOBJECT_END_OF_JOB_TIME_INFORMATION structure is carried out. By default,
        ///     all processes are terminated and the status code is set to
        ///     ERROR_NOT_ENOUGH_QUOTA.
        /// </summary>
        [FieldOffset(8)]
        public long PerJobUserTimeLimit;

        /// <summary>
        ///     Limit flags that are in effect. This member is a bit field that determines
        ///     whether other structure members are used. Any combination LimitFlag values
        ///     can be specified.
        /// </summary>
        [FieldOffset(16)]
        public LimitFlags LimitFlags;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_WORKINGSET, this member is the
        ///     minimum working set size for each process associated with the job. Otherwise,
        ///     this member is ignored.  If MaximumWorkingSetSize is nonzero,
        ///     MinimumWorkingSetSize cannot be zero.
        /// </summary>
        [FieldOffset(24)]
        public ulong MinimumWorkingSetSize;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_WORKINGSET, this member is the
        ///     maximum working set size for each process associated with the job. Otherwise,
        ///     this member is ignored.  If MinimumWorkingSetSize is nonzero,
        ///     MaximumWorkingSetSize cannot be zero.
        /// </summary>
        [FieldOffset(32)]
        public ulong MaximumWorkingSetSize;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_ACTIVE_PROCESS, this member is the
        ///     active process limit for the job. Otherwise, this member is ignored.  If you
        ///     try to associate a process with a job, and this causes the active process
        ///     count to exceed this limit, the process is terminated and the association
        ///     fails.
        /// </summary>
        [FieldOffset(40)]
        public int ActiveProcessLimit;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_AFFINITY, this member is the
        ///     processor affinity for all processes associated with the job. Otherwise,
        ///     this member is ignored.  The affinity must be a proper subset of the system
        ///     affinity mask obtained by calling the GetProcessAffinityMask function. The
        ///     affinity of each thread is set to this value, but threads are free to
        ///     subsequently set their affinity, as long as it is a subset of the specified
        ///     affinity mask. Processes cannot set their own affinity mask.
        /// </summary>
        [FieldOffset(48)]
        public IntPtr Affinity;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_PRIORITY_CLASS, this member is the
        ///     priority class for all processes associated with the job. Otherwise, this
        ///     member is ignored. Processes and threads cannot modify their priority class.
        ///     The calling process must enable the SE_INC_BASE_PRIORITY_NAME privilege.
        /// </summary>
        [FieldOffset(56)]
        public int PriorityClass;

        /// <summary>
        ///     If LimitFlags specifies JOB_OBJECT_LIMIT_SCHEDULING_CLASS, this member is
        ///     the scheduling class for all processes associated with the job. Otherwise,
        ///     this member is ignored.  The valid values are 0 to 9. Use 0 for the least
        ///     favorable scheduling class relative to other threads, and 9 for the most
        ///     favorable scheduling class relative to other threads. By default, this
        ///     value is 5. To use a scheduling class greater than 5, the calling process
        ///     must enable the SE_INC_BASE_PRIORITY_NAME privilege.
        /// </summary>
        [FieldOffset(60)]
        public int SchedulingClass;
    }

    #endregion
}