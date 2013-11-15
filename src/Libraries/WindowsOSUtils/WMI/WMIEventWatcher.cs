using System.Management;

namespace WindowsOSUtils.WMI
{
    public class WMIEventWatcher<T> : WMIWatcher<T>
    {
        /// <summary>
        /// Invoked asynchronously when the event being listened for occurs.
        /// </summary>
        public event WMIEventHandler<T> EventOccurred;

        /// <summary>
        /// Constructs a new WMIWatcher that watches for WMI events related to the WMI class specified by the type parameter.
        /// </summary>
        public WMIEventWatcher()
        {
            var wmiClassName = WMIUtils.GetWMIClassName<T>();
            var query = string.Format("SELECT * FROM {0}", wmiClassName);

            var watcher = new ManagementEventWatcher();
            watcher.EventArrived += HandleEvent;
            watcher.Query = new WqlEventQuery(query);

            Watchers.Add(watcher);
        }

        private void HandleEvent(object sender, EventArrivedEventArgs args)
        {
            if (EventOccurred == null) return;
            T instance = WMIUtils.FromEvent<T>(args);
            EventOccurred(instance);
        }
    }
}