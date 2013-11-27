using System;
using System.Windows.Forms;
using BDHero.Config;
using BDHero.Startup;
using BDHeroGUI.Properties;
using DotNetUtils.FS;
using Ninject;
using OSUtils.JobObjects;

namespace BDHeroGUI
{
    static class Program
    {
        private const string LogConfigFileName = "bdhero-gui.log.config";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var kernel = CreateInjector();
            var manager = kernel.Get<IJobObjectManager>();

            if (manager.TryBypassPCA(args))
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(kernel.Get<FormMain>());

            kernel.Get<ITempFileRegistrar>().DeleteEverything();
        }

        private static IKernel CreateInjector()
        {
            var kernel = InjectorFactory.CreateContainer();
            kernel.Get<LogInitializer>().Initialize(LogConfigFileName, Resources.log4net_config);
            kernel.Bind<FormMain>().ToSelf();
            return kernel;
        }
    }
}
