using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global
namespace NativeAPI.Mac.Memory
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct vm_statistics_data_t
    {
        public uint free_count;
        public uint active_count;
        public uint inactive_count;
        public uint wire_count;
        public uint zero_fill_count;
        public uint reactivations;
        public uint pageins;
        public uint pageouts;
        public uint faults;
        public uint cow_faults;
        public uint lookups;
        public uint hits;
        public uint purgeable_count;
        public uint purges;
        public uint speculative_count;
    }
}