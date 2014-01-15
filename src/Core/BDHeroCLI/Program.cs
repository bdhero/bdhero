// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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
