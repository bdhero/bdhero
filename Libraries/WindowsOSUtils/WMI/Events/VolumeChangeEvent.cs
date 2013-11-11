namespace WindowsOSUtils.WMI.Events
{
    /// <summary>
    /// The Win32_VolumeChangeEvent WMI class represents a local drive event that results from the addition of a drive letter or mounted drive on the computer system. Network drives are not currently supported.
    /// The following syntax is simplified from Managed Object Format (MOF) code and includes all of the inherited properties. Properties and methods are in alphabetic order, not MOF order.
    /// </summary>
    /// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/aa394516(v=vs.85).aspx"/>
    [WMIClassName("Win32_VolumeChangeEvent")]
    public struct VolumeChangeEvent
    {
        public VolumeChangeEvent(string driveName, VolumeChangeEventType eventType)
        {
            DriveName = driveName;
            EventType = eventType;
        }

        /// <summary>
        /// Drive name (letter) that has been added or removed from the system.
        /// </summary>
        /// <example>E:</example>
        public readonly string DriveName;

        /// <summary>
        /// Type of event. This property is inherited from Win32_DeviceChangeEvent.
        /// </summary>
        public readonly VolumeChangeEventType EventType;
    }
}
