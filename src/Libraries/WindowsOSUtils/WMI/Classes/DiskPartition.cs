// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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
    [WMIClassName("Win32_DiskPartition")]
    public class DiskPartition /* : CIM_DiskPartition */
    {
        public UInt16   Access;
        public UInt16   Availability;
        public UInt64   BlockSize;
        public bool     Bootable;
        public bool     BootPartition;
        public string   Caption;
        public UInt32   ConfigManagerErrorCode;
        public bool     ConfigManagerUserConfig;
        public string   CreationClassName;
        public string   Description;
        public string   DeviceID;
        public UInt32   DiskIndex;
        public bool     ErrorCleared;
        public string   ErrorDescription;
        public string   ErrorMethodology;
        public UInt32   HiddenSectors;
        public UInt32   Index;
        public DateTime InstallDate;
        public UInt32   LastErrorCode;
        public string   Name;
        public UInt64   NumberOfBlocks;
        public string   PNPDeviceID;
        public UInt16[] PowerManagementCapabilities;
        public bool     PowerManagementSupported;
        public bool     PrimaryPartition;
        public string   Purpose;
        public bool     RewritePartition;
        public UInt64   Size;
        public UInt64   StartingOffset;
        public string   Status;
        public UInt16   StatusInfo;
        public string   SystemCreationClassName;
        public string   SystemName;
        public string   Type;
    }
}
