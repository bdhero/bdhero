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

namespace WindowsOSUtils.WMI.Classes
{
    /// <summary>
    /// The Win32_LogicalDisk WMI class represents a data source that resolves to
    /// an actual local storage device on a computer system running Windows.
    /// </summary>
    [WMIClassName("Win32_LogicalDisk")]
    public class LogicalDisk /* : CIM_LogicalDisk */
    {
        public UInt16   Access;
        public UInt16   Availability;
        public UInt64   BlockSize;

        /// <summary>
        /// Short description of the object—a one-line string.
        /// This property is inherited from CIM_ManagedSystemElement.
        /// </summary>
        /// <example><c>"E:"</c></example>
        public string   Caption;

        public bool     Compressed;
        public UInt32   ConfigManagerErrorCode;
        public bool     ConfigManagerUserConfig;
        public string   CreationClassName;

        /// <summary>
        /// Description of the object.
        /// </summary>
        /// <example><c>"Local Fixed Disk"</c></example>
        public string   Description;

        /// <summary>
        /// Unique identifier of the logical disk from other devices on the system.
        /// This property is inherited from CIM_LogicalDevice.
        /// </summary>
        /// <example><c>"E:"</c></example>
        public string   DeviceID;

        /// <summary>
        /// The type of disk drive this logical disk represents.
        /// </summary>
        public LogicalDiskDriveType DriveType;

        public bool     ErrorCleared;
        public string   ErrorDescription;
        public string   ErrorMethodology;

        /// <summary>
        /// File system on the logical disk.
        /// </summary>
        /// <example><c>"NTFS"</c></example>
        /// <example><c>"FAT"</c></example>
        public string   FileSystem;

        /// <summary>
        /// Space, in bytes, available on the logical disk.
        /// This property is inherited from CIM_LogicalDisk.
        /// </summary>
        public UInt64   FreeSpace;

        public DateTime InstallDate;
        public UInt32   LastErrorCode;
        public UInt32   MaximumComponentLength;
        public UInt32   MediaType;

        /// <summary>
        /// Label by which the object is known.
        /// When subclassed, this property can be overridden to be a key property.
        /// This property is inherited from CIM_ManagedSystemElement.
        /// </summary>
        /// <example><c>"E:"</c></example>
        public string   Name;

        public UInt64   NumberOfBlocks;
        public string   PNPDeviceID;
        public UInt16[] PowerManagementCapabilities;
        public bool     PowerManagementSupported;
        public string   ProviderName;
        public string   Purpose;
        public bool     QuotasDisabled;
        public bool     QuotasIncomplete;
        public bool     QuotasRebuilding;

        /// <summary>
        /// Size of the disk drive in bytes.
        /// This property is inherited from CIM_LogicalDisk.
        /// </summary>
        public UInt64   Size;

        public string   Status;
        public UInt16   StatusInfo;
        public bool     SupportsDiskQuotas;
        public bool     SupportsFileBasedCompression;
        public string   SystemCreationClassName;
        public string   SystemName;
        public bool     VolumeDirty;

        /// <summary>
        /// Volume name of the logical disk.
        /// </summary>
        /// <example><c>"LEXAR"</c></example>
        public string   VolumeName;

        /// <summary>
        /// Volume serial number of the logical disk.
        /// Constraints: Maximum 11 characters.
        /// </summary>
        /// <example><c>"FCD27453"</c></example>
        public string   VolumeSerialNumber;
    }
}
