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
