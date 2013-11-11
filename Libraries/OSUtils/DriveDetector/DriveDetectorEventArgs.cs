using System;
using System.IO;

namespace OSUtils.DriveDetector
{
    /// <summary>
    /// Our class for passing in custom arguments to our event handlers.
    /// </summary>
    public class DriveDetectorEventArgs : EventArgs
    {
        /// <summary>
        /// Information about the drive.
        /// </summary>
        public DriveInfo DriveInfo;
    }
}