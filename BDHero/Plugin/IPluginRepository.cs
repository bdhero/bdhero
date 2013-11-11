using System.Collections.Generic;

namespace BDHero.Plugin
{
    /// <summary>
    ///     Maintains a collection of <see cref="IPlugin"/>s, organized by type.
    /// </summary>
    public interface IPluginRepository : IPluginHost
    {
        int Count { get; }

        IList<IPlugin> PluginsByType { get; }

        IList<IDiscReaderPlugin> DiscReaderPlugins { get; }
        IList<IMetadataProviderPlugin> MetadataProviderPlugins { get; }
        IList<IAutoDetectorPlugin> AutoDetectorPlugins { get; }
        IList<INameProviderPlugin> NameProviderPlugins { get; }
        IList<IMuxerPlugin> MuxerPlugins { get; }
        IList<IPostProcessorPlugin> PostProcessorPlugins { get; }

        void Clear();

        void Add(IPlugin plugin);
    }
}