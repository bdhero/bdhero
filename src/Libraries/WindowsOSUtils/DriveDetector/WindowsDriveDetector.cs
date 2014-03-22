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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DotNetUtils.Annotations;
using OSUtils.DriveDetector;
using NativeAPI.Win.Device;
using NativeAPI.Win.User;

namespace WindowsOSUtils.DriveDetector
{
    /// <summary>
    ///     A Windows-specific implementation of the <see cref="IDriveDetector"/> interface.
    /// </summary>
    /// <remarks>
    ///     Based on the CodeProject article "<a href="http://www.codeproject.com/Articles/18062/Detecting-USB-Drive-Removal-in-a-C-Program">Detecting USB Drive Removal in a C# Program</a>".
    /// </remarks>
    [UsedImplicitly]
    public class WindowsDriveDetector : IDriveDetector
    {
        public event DriveDetectorEventHandler DeviceArrived;
        public event DriveDetectorEventHandler DeviceRemoved;

        public void WndProc(ref Message m)
        {
            WindowMessage msg = m;

            if (!IsDeviceChangeEvent(msg)) return;
            if (!IsArrivalOrRemovalEvent(msg)) return;
            if (!IsLogicalVolume(msg)) return;

            // WM_DEVICECHANGE can have several meanings depending on the WParam value...
            switch (msg.WParam)
            {
                case WParam.DBT_DEVICEARRIVAL:
                    HandleDeviceArrival(msg);
                    break;
                case WParam.DBT_DEVICEREMOVECOMPLETE:
                    HandleDeviceAfterRemove(msg);
                    break;
            }
        }

        /// <summary>
        /// New device has just arrived.
        /// </summary>
        private void HandleDeviceArrival(WindowMessage msg)
        {
            Notify(DeviceArrived, msg);
        }

        /// <summary>
        /// Device has been removed.
        /// </summary>
        private void HandleDeviceAfterRemove(WindowMessage msg)
        {
            Notify(DeviceRemoved, msg);
        }

        private void Notify(DriveDetectorEventHandler handler, WindowMessage msg)
        {
            if (handler == null)
                return;

            var vol = msg.GetLParamAsStruct<DEV_BROADCAST_VOLUME>();
            var driveLetter = DriveMaskToLetter(vol.dbcv_unitmask);
            var drivePath = driveLetter + @":\";

            var args = new DriveDetectorEventArgs
                {
                    DriveInfo = DriveInfo.GetDrives().FirstOrDefault(info => info.Name == drivePath)
                };

            handler(this, args);
        }

        #region Win32 helpers

        private static bool IsDeviceChangeEvent(WindowMessage msg)
        {
            return msg.Is(WindowMessageType.WM_DEVICECHANGE);
        }

        private static bool IsArrivalOrRemovalEvent(WindowMessage msg)
        {
            return msg.IsOneOf(WParam.DBT_DEVICEARRIVAL,
                               WParam.DBT_DEVICEREMOVECOMPLETE);
        }

        private static bool IsLogicalVolume(WindowMessage msg)
        {
            return msg.Is(LParam.DBT_DEVTYP_VOLUME);
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
    }
}
