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
            Application.Run(CreateFormMain(args, kernel));

            kernel.Get<ITempFileRegistrar>().DeleteEverything();
        }

        private static FormMain CreateFormMain(string[] args, IKernel kernel)
        {
            var formMain = kernel.Get<FormMain>();
            formMain.Args = args;
            return formMain;
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
