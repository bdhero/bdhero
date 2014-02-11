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
