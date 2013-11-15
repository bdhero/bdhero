using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using BDHero.Startup;
using NUnit.Framework;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using UpdateLib;
using log4net;

namespace UpdaterTests
{
    [TestFixture]
    public class UpdaterTest
    {
        private readonly IKernel _kernel = TestInjectorFactory.CreateContainer();

        private Updater _updater;

        [SetUp]
        public void SetUpClient()
        {
            _updater = _kernel.Get<Updater>();
            _updater.IsPortable = false;
            _updater.BeforeRequest += UpdaterOnBeforeRequest;
        }

        [TearDown]
        public void TearDownClient()
        {
            if (_updater != null)
                _updater.BeforeRequest -= UpdaterOnBeforeRequest;
        }

        private void UpdaterOnBeforeRequest(HttpWebRequest request)
        {
            var timeout = (int) TimeSpan.FromSeconds(15).TotalMilliseconds;
            request.Timeout = timeout;
            request.ReadWriteTimeout = timeout;
        }

        [Test]
        public void TestIsUpdateAvailable()
        {
            _updater.CheckForUpdate(new Version(0, 0, 0, 0));
            Assert.IsTrue(_updater.IsUpdateAvailable,
                "Update SHOULD be available when running an old version");

            _updater.CheckForUpdate(_updater.LatestUpdate.Version);
            Assert.IsFalse(_updater.IsUpdateAvailable,
                "Update should NOT be available when already running the latest version");

            _updater.CheckForUpdate(new Version(99, 99, 99, 99));
            Assert.IsFalse(_updater.IsUpdateAvailable,
                "Update should NOT be available when running a newer version than the latest available");
        }

        [Test]
        public void TestDownloadIntegrity()
        {
            _updater.CheckForUpdate(new Version(0, 0, 0, 0));
            Console.WriteLine("Downloading v{0}", _updater.LatestUpdate.Version);
            _updater.DownloadUpdate();
            Console.WriteLine("Successfully downloaded update file");
        }

        // TODO: Test exception throwing
    }

    static class TestInjectorFactory
    {
        public static IKernel CreateContainer()
        {
            return new StandardKernel(new TestModule());
        }
    }

    /// <summary>
    /// Module used by unit tests.
    /// </summary>
    class TestModule : NinjectModule
    {
        public override void Load()
        {
            BindStartupDependencies();
        }

        private void BindStartupDependencies()
        {
            Bind<IDirectoryLocator>().To<DirectoryLocator>();
            Bind<ILog>().ToMethod(CreateLogger);
        }

        private static ILog CreateLogger(IContext context)
        {
            return LogManager.GetLogger(context.Request.Target.Type);
        }
    }
}
