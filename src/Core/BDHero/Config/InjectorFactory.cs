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

using System.Collections.Generic;
using WindowsOSUtils;
using WindowsOSUtils.Net;
using BDHero.Plugin;
using BDHero.Prefs;
using BDHero.Startup;
using DotNetUtils.FS;
using Ninject;
using Ninject.Modules;
using OSUtils;
using OSUtils.Info;
using OSUtils.Net;
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
            modules.Add(new NetworkModule());
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

    internal class NetworkModule : NinjectModule
    {
        public override void Load()
        {
            if (Windows7NetworkStatusMonitor.IsPlatformSupported)
                Bind<INetworkStatusMonitor>().To<Windows7NetworkStatusMonitor>().InSingletonScope();
            else
                Bind<INetworkStatusMonitor>().To<GenericNetworkStatusMonitor>().InSingletonScope();
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
            Bind<IPreferenceManager>().To<PreferenceManager>().InSingletonScope();
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
