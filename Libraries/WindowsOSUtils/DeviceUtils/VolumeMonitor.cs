using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using WindowsOSUtils.WMI;
using WindowsOSUtils.WMI.Classes;
using WindowsOSUtils.WMI.Events;

namespace WindowsOSUtils.DeviceUtils
{
    public class VolumeMonitor
    {
        // public event DeviceOperationHandler DeviceInserted;
        // public event DeviceOperationHandler DeviceRemoved;

        // public event DeviceOperationHandler DiskInserted;
        // public event DeviceOperationHandler DiskRemoved;

        public VolumeMonitor()
        {
            var eventWatcher1 = new WMIEventWatcher<VolumeChangeEvent>();
            var eventWatcher2 = new WMIEventWatcher<DeviceChangeEvent>();
            var instanceWatcher1 = new WMIInstanceWatcher<DiskDrive>();
            var instanceWatcher2 = new WMIInstanceWatcher<LogicalDisk>();

            eventWatcher1.EventOccurred += delegate(VolumeChangeEvent instance)
                {
                    Console.WriteLine(instance);
                };

            eventWatcher2.EventOccurred += delegate(DeviceChangeEvent instance)
                {
                    Console.WriteLine(instance);
                };

            instanceWatcher1.InstanceCreated += delegate(DiskDrive instance)
                {
                    Console.WriteLine(instance);
                };
            instanceWatcher1.InstanceDeleted += delegate(DiskDrive instance)
                {
                    Console.WriteLine(instance);
                };

            instanceWatcher2.InstanceCreated += delegate(LogicalDisk instance)
                {
                    Console.WriteLine(instance);
                };
            instanceWatcher2.InstanceDeleted += delegate(LogicalDisk instance)
                {
                    Console.WriteLine(instance);
                };

            eventWatcher1.Start();
            eventWatcher2.Start();
            instanceWatcher1.Start();
            instanceWatcher2.Start();
        }
    }

    public delegate void DeviceOperationHandler(string driveLetter);
}
