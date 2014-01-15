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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

// ReSharper disable ClassNeverInstantiated.Global
namespace BDHero.Startup
{
    public class LogInitializer
    {
        private readonly IDirectoryLocator _directoryLocator;

        private static log4net.ILog Logger
        {
            get { return log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); }
        }

        public LogInitializer(IDirectoryLocator directoryLocator)
        {
            _directoryLocator = directoryLocator;
        }

        public LogInitializer Initialize(string logConfigFileName, string defaultLogConfig)
        {
            var assemblyMeta = System.Reflection.Assembly.GetEntryAssembly().GetName();
            var logConfigPath = Path.Combine(_directoryLocator.AppConfigDir, logConfigFileName);

            log4net.GlobalContext.Properties["logdir"] = _directoryLocator.LogDir;
            log4net.GlobalContext.Properties["pid"] = Process.GetCurrentProcess().Id;

            EnsureLogConfigFileExists(logConfigPath, defaultLogConfig);

            if (File.Exists(logConfigPath))
            {
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(logConfigPath));
            }

            Logger.InfoFormat("{0} v{1} starting up", assemblyMeta.Name, assemblyMeta.Version);

            return this;
        }

        private static void EnsureLogConfigFileExists(string logConfigPath, string defaultLogConfig)
        {
            if (File.Exists(logConfigPath)) return;
            try
            {
                File.WriteAllText(logConfigPath, defaultLogConfig);
            }
            catch (Exception e)
            {
                log4net.Config.XmlConfigurator.Configure(new MemoryStream(Encoding.UTF8.GetBytes(defaultLogConfig), false));
                Logger.Error("Unable to create log4net config file", e);
            }
        }
    }
}
