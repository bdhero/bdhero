// Copyright 2014 Andrew C. Dvorak
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
using System.Net.NetworkInformation;
using System.Threading;
using DotNetUtils.Annotations;
using DotNetUtils.TaskUtils;
using Microsoft.WindowsAPICodePack.Net;
using OSUtils.Net;

namespace WindowsOSUtils.Net
{
    /// <summary>
    ///     Concrete implementation of the <see cref="INetworkStatusMonitor"/> interface that supports Windows Vista and newer.
    ///     Faster and more accurate than <see cref="GenericNetworkStatusMonitor"/>.
    /// </summary>
    [UsedImplicitly]
    public class Windows7NetworkStatusMonitor : INetworkStatusMonitor
    {
        /// <summary>
        ///     Gets whether the current operating system is supported by this class.
        /// </summary>
        public static bool IsPlatformSupported
        {
            get
            {
                try
                {
                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    return NetworkListManager.IsConnectedToInternet || true;
                }
                catch (PlatformNotSupportedException)
                {
                    return false;
                }
            }
        }

#pragma warning disable 1591

        public bool IsOnline
        {
            get { return NetworkListManager.IsConnectedToInternet; }
        }

        public event NetworkStatusChangedEventHandler NetworkStatusChanged;

        public void TestConnectionAsync()
        {
            new TaskBuilder()
                .OnCurrentThread()
                .DoWork(DoNothing)
                .Finally(NotifyObservers)
                .Build()
                .Start();
        }

#pragma warning restore 1591

        /// <summary>
        ///     Constructs a new <see cref="Windows7NetworkStatusMonitor"/> object.
        /// </summary>
        public Windows7NetworkStatusMonitor()
        {
            NetworkChange.NetworkAddressChanged += (s, e) => NotifyObservers();
            NetworkChange.NetworkAvailabilityChanged += (sender, args) => NotifyObservers();
        }

        private void NotifyObservers()
        {
            if (NetworkStatusChanged != null)
                NetworkStatusChanged(IsOnline);
        }

        private void DoNothing(IThreadInvoker threadinvoker, CancellationToken cancellationtoken)
        {
        }
    }
}