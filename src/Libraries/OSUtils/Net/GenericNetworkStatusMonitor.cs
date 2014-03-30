// Copyright 2013-2014 Andrew C. Dvorak
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

using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using DotNetUtils;
using DotNetUtils.Annotations;
using DotNetUtils.Concurrency;
using DotNetUtils.TaskUtils;

namespace OSUtils.Net
{
    /// <summary>
    ///     Concrete implementation of the <see cref="INetworkStatusMonitor"/> interface that supports all OSes,
    ///     though is likely slower and less accurate than <c>Windows7NetworkStatusMonitor</c>.
    /// </summary>
    [UsedImplicitly]
    public class GenericNetworkStatusMonitor : INetworkStatusMonitor
    {
#pragma warning disable 1591

        public bool IsOnline { get; private set; }

        public event NetworkStatusChangedEventHandler NetworkStatusChanged;

        public void TestConnectionAsync()
        {
            TestConnectionAsync(true);
        }

#pragma warning restore 1591

        /// <summary>
        ///     Constructs a new <c>Windows7NetworkStatusMonitor</c> object.
        /// </summary>
        public GenericNetworkStatusMonitor()
        {
            NetworkChange.NetworkAddressChanged += (s, e) => TestConnectionAsync(true);
            NetworkChange.NetworkAvailabilityChanged += (sender, args) => TestConnectionAsync(true);
        }

        private void TestConnectionAsync(bool notify)
        {
            var promise =
                new SimplePromise()
                    .Work(TestConnection)
                    .Fail(Fail)
                    .Canceled(Fail)
                    .Done(Done)
                ;

            if (notify)
            {
                promise.Always(NotifyObservers);
            }

            promise.Start();
        }

        private static void TestConnection(IPromise<bool> promise)
        {
            // ReSharper disable once UnusedVariable
            using (var client = new WebClient())
            using (var stream = client.OpenRead("http://www.google.com/"))
            {
            }

            // ReSharper disable once UnusedVariable
            using (var client = new WebClient())
            using (var stream = client.OpenRead("http://www.microsoft.com/"))
            {
            }
        }

        private void Fail(IPromise<bool> promise)
        {
            IsOnline = false;
        }

        private void Done(IPromise<bool> promise)
        {
            IsOnline = true;
        }

        private void NotifyObservers(IPromise<bool> promise)
        {
            if (NetworkStatusChanged != null)
                NetworkStatusChanged(IsOnline);
        }
    }
}