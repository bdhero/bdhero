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
