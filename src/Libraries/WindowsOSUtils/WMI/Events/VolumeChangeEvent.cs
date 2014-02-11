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
