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
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using BDHero.BDROM;
using BDHero.JobQueue;
using DotNetUtils.Annotations;

namespace BDHero.Plugin
{
    public delegate DialogResult EditPluginPreferenceHandler(Form parent);

    public interface IPlugin
    {
        IPluginHost Host { get; }

        /// <summary>
        /// Contains information about the plugin DLL and config file.
        /// </summary>
        PluginAssemblyInfo AssemblyInfo { get; }

        /// <summary>
        /// Human-friendly name of the plugin that will be displayed in the UI.
        /// </summary>
        string Name { get; }

        bool Enabled { get; set; }

        [CanBeNull]
        Icon Icon { get; }

        /// <summary>
        /// Gets the order in which to run the plugin relative to other plugins of the same type.
        /// Lower numbers run first; ties are resolved alphabetically by the name of the plugin.
        /// </summary>
        int RunOrder { get; }

        EditPluginPreferenceHandler EditPreferences { get; }

        /// <summary>
        /// Invoked when the application first starts up and loads the plugin assembly.
        /// </summary>
        /// <param name="host">Host object that loaded the plugin</param>
        /// <param name="assemblyInfo">Contains information about the plugin DLL and config file</param>
        void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo);

        /// <summary>
        /// Invoked when the application is about to exit.
        /// </summary>
        void UnloadPlugin();
    }

    public interface IDiscReaderPlugin : IPlugin
    {
        Disc ReadBDROM(CancellationToken cancellationToken, string bdromPath);
    }

    public interface IMetadataProviderPlugin : IPlugin
    {
        void GetMetadata(CancellationToken cancellationToken, Job job);
    }

    public interface IAutoDetectorPlugin : IPlugin
    {
        void AutoDetect(CancellationToken cancellationToken, Job job);
    }

    public interface INameProviderPlugin : IPlugin
    {
        void Rename(CancellationToken cancellationToken, Job job);
    }

    public interface IMuxerPlugin : IPlugin
    {
        MatroskaFeatures SupportedFeatures { get; }

        void Mux(CancellationToken cancellationToken, Job job);
    }

    public interface IPostProcessorPlugin : IPlugin
    {
        void PostProcess(CancellationToken cancellationToken, Job job);
    }

    [Flags]
    public enum MatroskaFeatures
    {
        None        = 0x00,
        Chapters    = 0x01,
        CoverArt    = 0x02,
        LPCM        = 0x04,
        DefaultFlag = 0x08,
        ForcedFlag  = 0x10,
    }

    /// <summary>
    /// Contains information about the plugin DLL and config file.
    /// </summary>
    public class PluginAssemblyInfo
    {
        /// <summary>
        /// Gets the full path to the plugin DLL.
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        /// Gets the version of the plugin assembly DLL.
        /// </summary>
        public Version Version { get; private set; }

        public DateTime BuildDate { get; private set; }

        /// <summary>
        /// Gets the GUID of the plugin assembly DLL.
        /// </summary>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Gets the full path to the plugin's JSON config file.
        /// </summary>
        public string ConfigFilePath { get; private set; }

        public PluginAssemblyInfo(string location, Version version, DateTime buildDate, Guid guid, string configFilePath)
        {
            Location = location;
            Version = version;
            BuildDate = buildDate;
            Guid = guid;
            ConfigFilePath = configFilePath;
        }
    }
}
