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
using System.Management;

namespace WindowsOSUtils.WMI
{
    public class WMIInstanceWatcher<T> : WMIWatcher<T>
    {
        public event WMIEventHandler<T> InstanceCreated;
        public event WMIEventHandler<T> InstanceDeleted;

        /// <summary>
        /// Creates a new WMIInstanceWatcher that polls for new events every 5 seconds.
        /// </summary>
        public WMIInstanceWatcher() : this(TimeSpan.FromSeconds(5))
        {
        }

        /// <summary>
        /// Creates a new WMIInstanceWatcher with the specified poll interval.
        /// </summary>
        /// <param name="pollInterval">Polling interval between checks for new events</param>
        public WMIInstanceWatcher(TimeSpan pollInterval)
        {
            var wmiClassName = WMIUtils.GetWMIClassName<T>();
            var condition = string.Format("(TargetInstance ISA '{0}')", wmiClassName);

            var createEventQuery = new WqlEventQuery
                {
                    EventClassName = "__InstanceCreationEvent",
                    WithinInterval = pollInterval,
                    Condition = condition
                };

            var deleteEventQuery = new WqlEventQuery
                {
                    EventClassName = "__InstanceDeletionEvent",
                    WithinInterval = pollInterval,
                    Condition = condition
                };

            var createWatcher = new ManagementEventWatcher();
            createWatcher.EventArrived += HandleCreateEvent;
            createWatcher.Query = createEventQuery;

            var deleteWatcher = new ManagementEventWatcher();
            deleteWatcher.EventArrived += HandleDeleteEvent;
            deleteWatcher.Query = deleteEventQuery;

            Watchers.Add(createWatcher);
            Watchers.Add(deleteWatcher);
        }

        private void HandleCreateEvent(object sender, EventArrivedEventArgs args)
        {
            if (InstanceCreated == null) return;
            var obj = args.NewEvent.GetPropertyValue("TargetInstance");
            T instance = WMIUtils.FromManagementObject<T>(obj as ManagementBaseObject);
            InstanceCreated(instance);
        }

        private void HandleDeleteEvent(object sender, EventArrivedEventArgs args)
        {
            if (InstanceDeleted == null) return;
            var obj = args.NewEvent.GetPropertyValue("TargetInstance");
            T instance = WMIUtils.FromManagementObject<T>(obj as ManagementBaseObject);
            InstanceCreated(instance);
        }
    }
}