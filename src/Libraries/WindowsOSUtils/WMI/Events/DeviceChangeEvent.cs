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
