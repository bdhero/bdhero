using BDHero.Config;
using BDHero.Startup;
using BDHeroCLI.Properties;
using DotNetUtils.FS;
using Ninject;
using OSUtils.JobObjects;

namespace BDHeroCLI
{
    static class Program
    {
        private const string LogConfigFileName = "bdhero-cli.log.config";

        static void Main(string[] args)
        {
            var kernel = CreateInjector();
            var manager = kernel.Get<IJobObjectManager>();

            if (manager.TryBypassPCA(args))
                return;

            kernel.Get<CLI>().Run(args);

            kernel.Get<ITempFileRegistrar>().DeleteEverything();
        }

        private static IKernel CreateInjector()
        {
            var kernel = InjectorFactory.CreateContainer();
            kernel.Get<LogInitializer>().Initialize(LogConfigFileName, Resources.log4net_config);
            kernel.Bind<CLI>().ToSelf();
            return kernel;
        }
    }
}
