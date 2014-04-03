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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DotNetUtils.Annotations;

namespace BDHero.Plugin
{
    /// <see cref="http://www.codeproject.com/Articles/6334/Plug-ins-in-C"/>
    [UsedImplicitly]
    public class PluginRepository : IPluginRepository
    {
        private readonly ConcurrentDictionary<Guid, ProgressProvider> _progressProviders =
            new ConcurrentDictionary<Guid, ProgressProvider>();

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

        public void ReportProgress(IPlugin plugin, double percentComplete, string shortStatus, string longStatus = null)
        {
            var progressProvider = GetProgressProvider(plugin);
            progressProvider.Update(percentComplete, shortStatus, longStatus);
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


