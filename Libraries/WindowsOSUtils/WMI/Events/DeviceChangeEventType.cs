namespace WindowsOSUtils.WMI.Events
{
    /// <summary>
    /// Type of event change notification that has occurred.
    /// </summary>
    public enum DeviceChangeEventType : uint
    {
        /// <summary>
        /// Configuration Changed
        /// </summary>
        ConfigurationChanged = 1,
        
        /// <summary>
        /// Device Arrival
        /// </summary>
        DeviceArrival =  2,

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