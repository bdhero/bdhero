using System;

namespace OSUtils.DriveDetector
{
    /// <summary>
    /// Delegate for event handler to handle the device events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DriveDetectorEventHandler(Object sender, DriveDetectorEventArgs e);
}