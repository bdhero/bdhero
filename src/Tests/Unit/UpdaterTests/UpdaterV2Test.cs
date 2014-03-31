using System;
using System.Net;
using System.Threading;
using BDHero.Startup;
using NUnit.Framework;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using UpdateLib;
using log4net;
using UpdateLib.V2;

namespace UpdaterTests
{
    [TestFixture]
    public class UpdaterV2Test
    {
        private UpdaterV2 _updater;
        private ManualResetEventSlim _barrier;
        private string _events;

        [SetUp]
        public void SetUpClient()
        {
            _updater = new UpdaterV2();
            _barrier = new ManualResetEventSlim();
            _events = "";

            _updater.Checking += updater => _events += "Checking;";
            _updater.UpdateFound += updater => _events += "UpdateFound;";
            _updater.UpdateNotFound += updater => _events += "UpdateNotFound;";
            _updater.Checked += updater => _events += "Checked;";
            _updater.Error += (updater, exception) => _events += "Error;";

            _updater.Checked += updater => _barrier.Set();
            _updater.Error += (updater, exception) => _barrier.Set();
        }

        [Test]
        public void TestUpdateFound()
        {
            _updater.CurrentVersion = new Version(0, 0, 0, 0);
            _updater.IsPortable = true;

            _updater.CheckForUpdateAsync();

            var gotResponse = _barrier.Wait(TimeSpan.FromSeconds(5));

            Assert.IsTrue(gotResponse, "Did not receive a response from the update server within 5 seconds");
            Assert.IsNotNull(_updater.LatestUpdate);
            Assert.IsTrue(_updater.IsUpdateAvailable, "Expected an update to be available");
            Assert.Greater(_updater.LatestUpdate.Version, _updater.CurrentVersion, "LatestUpdate.Version should be greater than CurrentVersion");
            Assert.AreEqual("Checking;UpdateFound;Checked;", _events);
            Assert.IsTrue(_updater.LatestUpdate.FileName.EndsWith(".zip"), "LatestUpdate.FileName should end with \".zip\"");
        }

        [Test]
        public void TestUpdateNotFound()
        {
            _updater.CurrentVersion = new Version(999, 0, 0, 0);
            _updater.IsPortable = true;

            _updater.CheckForUpdateAsync();

            var gotResponse = _barrier.Wait(TimeSpan.FromSeconds(5));

            Assert.IsTrue(gotResponse, "Did not receive a response from the update server within 5 seconds");
            Assert.IsNotNull(_updater.LatestUpdate);
            Assert.IsFalse(_updater.IsUpdateAvailable, "Expected no update to be available");
            Assert.AreEqual("Checking;UpdateNotFound;Checked;", _events);
        }

        [Test]
        public void TestError()
        {
            _updater.CurrentVersion = new Version(0, 0, 0, 0);
            _updater.IsPortable = true;
            _updater.UpdateManifestFilePath += "_404";

            _updater.CheckForUpdateAsync();

            var gotResponse = _barrier.Wait(TimeSpan.FromSeconds(5));

            Assert.IsTrue(gotResponse, "Did not receive a response from the update server within 5 seconds");
            Assert.IsNull(_updater.LatestUpdate);
            Assert.IsFalse(_updater.IsUpdateAvailable, "Expected no update to be available");
            Assert.AreEqual("Checking;Error;", _events);
        }

        // TODO: Test exception throwing
    }
}
