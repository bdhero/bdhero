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
using System.Linq;
using System.Text;
using BDHero.Exceptions;
using BDHero.Plugin;
using DotNetUtils.Annotations;
using log4net;

namespace BDHero.Startup
{
    public class PluginLoader
    {
        private readonly ILog _logger;
        private readonly IDirectoryLocator _directoryLocator;
        private readonly IPluginRepository _pluginRepository;
        private readonly IPluginService _pluginService;

        [UsedImplicitly]
        public PluginLoader(ILog logger, IDirectoryLocator directoryLocator, IPluginRepository pluginRepository, IPluginService pluginService)
        {
            _logger = logger;
            _directoryLocator = directoryLocator;
            _pluginRepository = pluginRepository;
            _pluginService = pluginService;
        }

        /// <exception cref="RequiredPluginNotFoundException{T}"></exception>
        public void LoadPlugins()
        {
            LoadPluginsFromService();
            VerifyPlugins();
        }

        private void LoadPluginsFromService()
        {
            _pluginService.UnloadPlugins();
            _pluginService.LoadPlugins(_directoryLocator.RequiredPluginDir);
            _pluginService.LoadPlugins(_directoryLocator.CustomPluginDir);
        }

        /// <summary>Checks that all required plugin types are loaded.</summary>
        /// <exception cref="RequiredPluginNotFoundException{T}"></exception>
        private void VerifyPlugins()
        {
            if (_pluginRepository.DiscReaderPlugins.Count == 0)
                throw new RequiredPluginNotFoundException<IDiscReaderPlugin>("A Disc Reader plugin is required");
            if (_pluginRepository.MuxerPlugins.Count == 0)
                throw new RequiredPluginNotFoundException<IMuxerPlugin>("A Muxer plugin is required");
        }

        public void LogPlugins()
        {
            _logger.InfoFormat("Loaded {0} plugins:", _pluginRepository.Count);
            LogPlugins("Disc Readers", _pluginRepository.DiscReaderPlugins);
            LogPlugins("Metadata Providers", _pluginRepository.MetadataProviderPlugins);
            LogPlugins("Auto Detectors", _pluginRepository.AutoDetectorPlugins);
            LogPlugins("Name Providers", _pluginRepository.NameProviderPlugins);
            LogPlugins("Muxers", _pluginRepository.MuxerPlugins);
            LogPlugins("Post Processors", _pluginRepository.PostProcessorPlugins);
        }

        private void LogPlugins<T>(string name, IList<T> plugins) where T : IPlugin
        {
            _logger.InfoFormat("\t {0} ({1}){2}", name, plugins.Count, plugins.Any() ? ":" : "");
            foreach (var plugin in plugins)
            {
                _logger.InfoFormat("\t\t {0} v{1} - {2} - {3}", plugin.Name, plugin.AssemblyInfo.Version, plugin.AssemblyInfo.Guid, plugin.AssemblyInfo.Location);
            }
        }
    }
}
