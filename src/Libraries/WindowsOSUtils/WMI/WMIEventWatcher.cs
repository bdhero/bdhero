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