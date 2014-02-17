using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
namespace WinAPI.Kernel
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct JobObjectBasicLimitInformation
    {
        public readonly Int64 PerProcessUserTimeLimit;
        public readonly Int64 PerJobUserTimeLimit;
        public LimitFlags LimitFlags;
        public readonly UIntPtr MinimumWorkingSetSize;
        public readonly UIntPtr MaximumWorkingSetSize;
        public readonly Int16 ActiveProcessLimit;
        public readonly Int64 Affinity;
        public readonly Int16 PriorityClass;
        public readonly Int16 SchedulingClass;
    }
}