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
