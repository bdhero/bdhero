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

namespace WindowsOSUtils.WMI.Classes
{
    [WMIClassName("Win32_DiskDrive")]
    public class DiskDrive /* : CIM_DiskDrive */
    {
        public UInt16   Availability;
        public UInt32   BytesPerSector;
        public UInt16[] Capabilities;
        public string[] CapabilityDescriptions;
        public string   Caption;
        public string   CompressionMethod;
        public UInt32   ConfigManagerErrorCode;
        public bool     ConfigManagerUserConfig;
        public string   CreationClassName;
        public UInt64   DefaultBlockSize;
        public string   Description;
        public string   DeviceID;
        public bool     ErrorCleared;
        public string   ErrorDescription;
        public string   ErrorMethodology;
        public string   FirmwareRevision;
        public UInt32   Index;
        public DateTime InstallDate;
        public string   InterfaceType;
        public UInt32   LastErrorCode;
        public string   Manufacturer;
        public UInt64   MaxBlockSize;
        public UInt64   MaxMediaSize;
        public bool     MediaLoaded;
        public string   MediaType;
        public UInt64   MinBlockSize;
        public string   Model;
        public string   Name;
        public bool     NeedsCleaning;
        public UInt32   NumberOfMediaSupported;
        public UInt32   Partitions;
        public string   PNPDeviceID;
        public UInt16[] PowerManagementCapabilities;
        public bool     PowerManagementSupported;
        public UInt32   SCSIBus;
        public UInt16   SCSILogicalUnit;
        public UInt16   SCSIPort;
        public UInt16   SCSITargetId;
        public UInt32   SectorsPerTrack;
        public string   SerialNumber;
        public UInt32   Signature;
        public UInt64   Size;
        public string   Status;
        public UInt16   StatusInfo;
        public string   SystemCreationClassName;
        public string   SystemName;
        public UInt64   TotalCylinders;
        public UInt32   TotalHeads;
        public UInt64   TotalSectors;
        public UInt64   TotalTracks;
        public UInt32   TracksPerCylinder;
    }
}
