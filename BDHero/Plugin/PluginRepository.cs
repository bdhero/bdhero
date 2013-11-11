﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetUtils.Annotations;

namespace BDHero.Plugin
{
    /// <see cref="http://www.codeproject.com/Articles/6334/Plug-ins-in-C"/>
    [UsedImplicitly]
    public class PluginRepository : IPluginRepository
    {
        private readonly ConcurrentDictionary<string, ProgressProvider> _progressProviders =
            new ConcurrentDictionary<string, ProgressProvider>();

        private readonly IList<IPlugin> _plugins = new List<IPlugin>();

        public int Count
        {
            get { return _plugins.Count; }
        }

        public IList<IPlugin> PluginsByType
        {
            get
            {
                var plugins = new List<IPlugin>();
                plugins.AddRange(DiscReaderPlugins);
                plugins.AddRange(MetadataProviderPlugins);
                plugins.AddRange(AutoDetectorPlugins);
                plugins.AddRange(NameProviderPlugins);
                plugins.AddRange(MuxerPlugins);
                plugins.AddRange(PostProcessorPlugins);
                return plugins;
            }
        }

        public IList<IDiscReaderPlugin>       DiscReaderPlugins       { get { return PluginsOfType<IDiscReaderPlugin>(); } }
        public IList<IMetadataProviderPlugin> MetadataProviderPlugins { get { return PluginsOfType<IMetadataProviderPlugin>(); } }
        public IList<IAutoDetectorPlugin>     AutoDetectorPlugins     { get { return PluginsOfType<IAutoDetectorPlugin>(); } }
        public IList<INameProviderPlugin>     NameProviderPlugins     { get { return PluginsOfType<INameProviderPlugin>(); } }
        public IList<IMuxerPlugin>            MuxerPlugins            { get { return PluginsOfType<IMuxerPlugin>(); } }
        public IList<IPostProcessorPlugin>    PostProcessorPlugins    { get { return PluginsOfType<IPostProcessorPlugin>(); } }

        public void ReportProgress(IPlugin plugin, double percentComplete, string status)
        {
            var progressProvider = GetProgressProvider(plugin);
            progressProvider.Update(percentComplete, status);
        }

        public ProgressProvider GetProgressProvider(IPlugin plugin)
        {
            var progressProvider = _progressProviders.GetOrAdd(plugin.AssemblyInfo.Guid, guid => new ProgressProvider());
            progressProvider.Plugin = plugin;
            return progressProvider;
        }

        public void Clear()
        {
            _plugins.Clear();
        }

        public void Add(IPlugin plugin)
        {
            _plugins.Add(plugin);
        }

        private IList<T> PluginsOfType<T>() where T : IPlugin
        {
            return _plugins.OfType<T>().OrderBy(plugin => plugin.RunOrder).ToList();
        }
    }
}


