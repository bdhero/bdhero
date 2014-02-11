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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using OSUtils.DriveDetector;

namespace WindowsOSUtils.DriveDetector
{
    /// <summary>
    ///     A Windows-specific implementation of the <see cref="IDriveDetector"/> interface.
    /// </summary>
    /// <remarks>
    ///     Based on the CodeProject article "<a href="http://www.codeproject.com/Articles/18062/Detecting-USB-Drive-Removal-in-a-C-Program">Detecting USB Drive Removal in a C# Program</a>".
    /// </remarks>
    public class WindowsDriveDetector : IDriveDetector
    {
        public event DriveDetectorEventHandler DeviceArrived;
        public event DriveDetectorEventHandler DeviceRemoved;

        public void WndProc(ref Message m)
        {
            if (!IsDeviceChangeEvent(m)) return;
            if (!IsArrivalOrRemovalEvent(m)) return;
            if (!IsLogicalVolume(m)) return;

            // WM_DEVICECHANGE can have several meanings depending on the WParam value...
            switch (GetEventType(m))
            {
                case DBT_DEVICEARRIVAL:
                    HandleDeviceArrival(m);
                    break;
                case DBT_DEVICEREMOVECOMPLETE:
                    HandleDeviceAfterRemove(m);
                    break;
            }
        }

        /// <summary>
        /// New device has just arrived.
        /// </summary>
        private void HandleDeviceArrival(Message m)
        {
            Notify(DeviceArrived, m);
        }

        /// <summary>
        /// Device has been removed.
        /// </summary>
        private void HandleDeviceAfterRemove(Message m)
        {
            Notify(DeviceRemoved, m);
        }

        private void Notify(DriveDetectorEventHandler handler, Message m)
        {
            if (handler == null)
                return;
            
            var vol = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));
            var driveLetter = DriveMaskToLetter(vol.dbcv_unitmask);
            var drivePath = driveLetter + @":\";

            var args = new DriveDetectorEventArgs
                {
                    DriveInfo = DriveInfo.GetDrives().FirstOrDefault(info => info.Name == drivePath)
                };

            handler(this, args);
        }

        #region Win32 helpers

        private static bool IsDeviceChangeEvent(Message m)
        {
            return WM_DEVICECHANGE == m.Msg;
        }

        private static bool IsArrivalOrRemovalEvent(Message m)
        {
            var eventType = GetEventType(m);
            return DBT_DEVICEARRIVAL == eventType
                || DBT_DEVICEREMOVECOMPLETE == eventType
            ;
        }

        private static bool IsLogicalVolume(Message m)
        {
            return DBT_DEVTYP_VOLUME == GetDeviceType(m);
        }

        private static int GetEventType(Message m)
        {
            return m.WParam.ToInt32();
        }

        private static int GetDeviceType(Message m)
        {
            return Marshal.ReadInt32(m.LParam, 4);
        }

        /// <summary>
        /// Gets drive letter from a bit mask where bit 0 = A, bit 1 = B etc.
        /// There can actually be more than one drive in the mask but we 
        /// just use the last one in this case.
        /// </summary>
        /// <param name="mask"></param>
        /// <returns>The drive letter represented by the bit mask</returns>
        private static char DriveMaskToLetter(int mask)
        {
            const string drives = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

//            // 1 = A
//            // 2 = B
//            // 4 = C...
//            int cnt = 0;
//            int pom = mask / 2;
//            while (pom != 0)
//            {
//                // while there is any bit set in the mask
//                // shift it to the righ...                
//                pom = pom / 2;
//                cnt++;
//            }
//
//            return cnt < drives.Length ? drives[cnt] : '?';

            int i;

            for (i = 0; i < 26; ++i)
            {
                if ((mask & 0x1) != 0)
                    break;
                mask = mask >> 1;
            }

            if (i >= drives.Length)
                throw new ArgumentOutOfRangeException("Unable to determine drive letter for mask value " + mask);

            return drives[i];
        }

        #endregion

        #region Win32 constants

// ReSharper disable InconsistentNaming

        /// <summary>
        ///     Notifies an application of a change to the hardware configuration of a device or the computer.
        /// </summary>
        private const int WM_DEVICECHANGE = 0x0219;
        
        /// <summary>
        ///     A device or piece of media has been inserted and is now available.
        /// </summary>
        private const int DBT_DEVICEARRIVAL = 0x8000;

        /// <summary>
        ///     Permission is requested to remove a device or piece of media.
        ///     Any application can deny this request and cancel the removal.
        /// </summary>
        private const int DBT_DEVICEQUERYREMOVE = 0x8001;

        /// <summary>
        ///     A device or piece of media has been removed.
        /// </summary>
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        
        /// <summary>
        ///     Drive type is logical volume.
        /// </summary>
        private const int DBT_DEVTYP_VOLUME = 0x00000002;

// ReSharper restore InconsistentNaming

        #endregion

        #region Win32 structures

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable FieldCanBeMadeReadOnly.Local

        /// <summary>
        ///     Struct for parameters of the <see cref="WM_DEVICECHANGE"/> message.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct DEV_BROADCAST_VOLUME
        {
            public int dbcv_size;
            public int dbcv_devicetype;
            public int dbcv_reserved;
            public int dbcv_unitmask;
        }

// ReSharper restore FieldCanBeMadeReadOnly.Local
// ReSharper restore MemberCanBePrivate.Local
// ReSharper restore InconsistentNaming

        #endregion
    }
}
