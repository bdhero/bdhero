namespace WindowsOSUtils.WMI.Classes
{
    /// <summary>
    /// Type of disk drive represented by a <see cref="LogicalDisk"/>.
    /// </summary>
    public enum LogicalDiskDriveType : uint
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// No Root Directory
        /// </summary>
        NoRootDirectory = 1,

        /// <summary>
        /// Removable Disk
        /// </summary>
        RemovableDisk = 2,

        /// <summary>
        /// Local Disk
        /// </summary>
        LocalDisk = 3,

        /// <summary>
        /// Network Drive
        /// </summary>
        NetworkDrive = 4,

        /// <summary>
        /// Compact Disc
        /// </summary>
        CompactDisc = 5,

        /// <summary>
        /// RAM Disk
        /// </summary>
        RAMDisk = 6
    }
}