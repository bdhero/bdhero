using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

// ReSharper disable ClassNeverInstantiated.Global
namespace BDHero.Startup
{
    public sealed class DirectoryLocator : IDirectoryLocator
    {
        private const string AppDataRootDirName = "BDHero";
        private const string ConfigDirName = "Config";
        private const string ApplicationDirName = "Application";
        private const string PluginDirName = "Plugins";
        private const string RequiredDirName = "Required";
        private const string CustomDirName = "Custom";
        private const string LogDirName = "Logs";

        public bool   IsPortable        { get; private set; }
        public string InstallDir        { get; private set; }
        public string AppConfigDir      { get; private set; }
        public string PluginConfigDir   { get; private set; }
        public string RequiredPluginDir { get; private set; }
        public string CustomPluginDir   { get; private set; }
        public string LogDir            { get; private set; }

        public DirectoryLocator()
        {
            InstallDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            Debug.Assert(InstallDir != null, "InstallDir != null");

            IsPortable = Directory.Exists(Path.Combine(InstallDir, ConfigDirName));

            if (IsPortable)
            {
                AppConfigDir = Path.Combine(InstallDir, ConfigDirName, ApplicationDirName);
                PluginConfigDir = Path.Combine(InstallDir, ConfigDirName, PluginDirName);
                RequiredPluginDir = Path.Combine(InstallDir, PluginDirName, RequiredDirName);
                CustomPluginDir = Path.Combine(InstallDir, PluginDirName, CustomDirName);
                LogDir = Path.Combine(InstallDir, LogDirName);
            }
            else
            {
                var roamingAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDataRootDirName);
                var localAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppDataRootDirName);
                AppConfigDir = Path.Combine(roamingAppData, ConfigDirName, ApplicationDirName);
                PluginConfigDir = Path.Combine(roamingAppData, ConfigDirName, PluginDirName);
                RequiredPluginDir = Path.Combine(InstallDir, PluginDirName);
                CustomPluginDir = Path.Combine(roamingAppData, PluginDirName);
                LogDir = Path.Combine(localAppData, LogDirName);
            }

            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
            }
        }
    }
}
