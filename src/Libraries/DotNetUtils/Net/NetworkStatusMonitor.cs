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
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using DotNetUtils.TaskUtils;

namespace DotNetUtils.Net
{
    public class NetworkStatusMonitor
    {
        private static readonly Regex IPRegex = new Regex(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$");

        public Action TestIsOnline;

        public event NetworkStatusChangedEventHandler NetworkStatusChanged;

        public NetworkStatusMonitor(Action testIsOnline = null, NetworkStatusChangedEventHandler networkStatusChanged = null)
        {
            TestIsOnline = testIsOnline ?? DefaultTestIsOnline;
            if (networkStatusChanged != null)
                NetworkStatusChanged += networkStatusChanged;
            NetworkChange.NetworkAddressChanged += (s, e) => OnNetworkAddressChanged();
            OnNetworkAddressChanged();
        }

        private void DefaultTestIsOnline()
        {
            TestIPv4("http://icanhazip.com/");
            TestIPv4("http://api.exip.org/?call=ip");
        }

        private void TestIPv4(string url)
        {
            var req = HttpRequest.BuildRequest(HttpRequestMethod.Get, url);
            var resp = HttpRequest.Get(req);
            if (!IPRegex.IsMatch(resp))
                throw new HttpListenerException();
        }

        private void OnNetworkAddressChanged()
        {
            if (TestIsOnline == null)
                return;

            new TaskBuilder()
                .OnCurrentThread()
                .DoWork(Work)
                .Fail(Fail)
                .Succeed(Succeed)
                .Build()
                .Start()
                ;
        }

        private void Work(IThreadInvoker threadInvoker, CancellationToken cancellationToken)
        {
            TestIsOnline();
        }

        private void Fail(ExceptionEventArgs args)
        {
            IsOnline(false);
        }

        private void Succeed()
        {
            IsOnline(true);
        }

        private void IsOnline(bool isOnline)
        {
            if (NetworkStatusChanged != null)
                NetworkStatusChanged(isOnline);
        }
    }

    public delegate void NetworkStatusChangedEventHandler(bool isOnline);
}
