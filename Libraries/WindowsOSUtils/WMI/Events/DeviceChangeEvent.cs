using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsOSUtils.WMI.Events
{
    /// <summary>
    /// The Win32_DeviceChangeEvent abstract WMI class represents device change events that result
    /// from the addition, removal, or modification of devices on the computer system.
    /// This includes changes in the hardware configuration (docking and undocking),
    /// the hardware state, or newly mapped devices (mapping of a network drive).
    /// For example, a device has changed when a WM_DEVICECHANGE message is sent.
    /// </summary>
    [WMIClassName("Win32_DeviceChangeEvent")]
    public class DeviceChangeEvent
    {
        /// <summary>
        /// Type of event change notification that has occurred.
        /// </summary>
        public DeviceChangeEventType EventType;
    }
}
