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
