namespace WindowsOSUtils.WMI.Events
{
    /// <summary>
    /// Type of event. This property is inherited from Win32_DeviceChangeEvent.
    /// </summary>
    /// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/aa394516(v=vs.85).aspx"/>
    public enum VolumeChangeEventType : uint
    {
        /// <summary>
        /// Configuration Changed
        /// </summary>
        ConfigurationChanged = 1,

        /// <summary>
        /// Device Arrival
        /// </summary>
        DeviceArrival = 2,

        /// <summary>
        /// Device Removal
        /// </summary>
        DeviceRemoval = 3,

        /// <summary>
        /// Docking
        /// </summary>
        Docking = 4
    }
}