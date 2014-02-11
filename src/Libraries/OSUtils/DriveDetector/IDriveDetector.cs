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
using System.Text;
using System.Windows.Forms;

namespace OSUtils.DriveDetector
{
    /// <summary>
    ///     Detects the insertion or removal of a removable logical volume (e.g., USB flash drive, external HDD, CD, DVD, Blu-ray Disc).
    /// </summary>
    /// <remarks>
    ///     Based on the CodeProject article "<a href="http://www.codeproject.com/Articles/18062/Detecting-USB-Drive-Removal-in-a-C-Program">Detecting USB Drive Removal in a C# Program</a>".
    /// </remarks>
    public interface IDriveDetector
    {
        /// <summary>
        /// Triggered whenever a removable drive of any type (USB flash, external HDD, CD, DVD, Blu-ray) is plugged in.
        /// </summary>
        event DriveDetectorEventHandler DeviceArrived;

        /// <summary>
        /// Triggered <strong>after</strong> a removable drive of any type (USB flash, external HDD, CD, DVD, Blu-ray) is removed.
        /// </summary>
        event DriveDetectorEventHandler DeviceRemoved;

        /// <summary>
        /// Message handler which must be called from client form.
        /// Processes Windows messages and calls event handlers. 
        /// </summary>
        /// <param name="m"></param>
        void WndProc(ref Message m);
    }
}
