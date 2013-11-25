using System.Collections.Generic;
using WindowsOSUtils;
using BDHero.Plugin;
using BDHero.Startup;
using DotNetUtils.FS;
using Ninject;
using Ninject.Modules;
using OSUtils;
using UpdateLib;
using log4net;

namespace BDHero.Config
{
    public static class InjectorFactory
    {
        public static IKernel CreateContainer()
        {
            var modules = new List<INinjectModule>();
            modules.Add(new LoggingModule());
            modules.Add(new BDHeroMainModule());
            modules.AddRange(CreateOSMainModules());
            return new StandardKernel(modules.ToArray());
        }

        private static IEnumerable<INinjectModule> CreateOSMainModules()
        {
            var osType = SystemInfo.Instance.OS.Type;
            return osType == OSType.Windows
                       ? WindowsInjectorFactory.CreateMainModules()
                       : MockOSInjectorFactory.CreateMainModules();
        }
    }

    internal class LoggingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILog>().ToMethod(context => LogManager.GetLogger(context.Request.Target.Type));
        }
    }

    internal class BDHeroMainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDirectoryLocator>().To<DirectoryLocator>().InSingletonScope();
            Bind<LogInitializer>().ToSelf().InSingletonScope();
            Bind<ITempFileRegistrar>().To<TempFileRegistrar>().InSingletonScope();
            Bind<IPluginRepository>().To<PluginRepository>().InSingletonScope();
            Bind<PluginLoader>().ToSelf().InSingletonScope();
            Bind<Updater>().ToSelf().InSingletonScope();
            Bind<IController>().To<Controller>();

            if (DevPluginService.IsDevMode)
            {
                Bind<IPluginService>().To <DevPluginService>().InSingletonScope();
            }
            else
            {
                Bind<IPluginService>().To<PluginService>().InSingletonScope();
            }
        }
    }
}
