using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using BDHero.JobQueue;
using BDHero.Plugin;

namespace IsanPlugin
{
    public class IsanPlugin : IMetadataProviderPlugin
    {
        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "ISAN Metadata Provider"; } }

        public bool Enabled { get; set; }

        public Icon Icon { get; private set; }

        public int RunOrder { get { return 0; } }

        public EditPluginPreferenceHandler EditPreferences { get; private set; }

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        public void GetMetadata(CancellationToken cancellationToken, Job job)
        {
            var raw = job.Disc.Metadata.Raw;
            var derived = job.Disc.Metadata.Derived;

            var token = new ProgressToken(Host, this, cancellationToken);
            var provider = new IsanMetadataProvider(token);

            provider.Populate(raw.V_ISAN);

            var isan = raw.ISAN;
            if (isan != null && !string.IsNullOrWhiteSpace(isan.Title))
            {
                // TODO: Scrape language from isan.org
                // Don't insert twice
                if (!derived.SearchQueries.Any(query => query.Title == isan.Title && query.Year == isan.Year))
                    derived.SearchQueries.Insert(0, new SearchQuery { Title = isan.Title, Year = isan.Year });
            }
        }
    }
}
