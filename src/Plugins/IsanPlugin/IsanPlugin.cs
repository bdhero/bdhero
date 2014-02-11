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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using BDHero.BDROM;
using BDHero.JobQueue;
using BDHero.Plugin;
using DotNetUtils.Annotations;
using Newtonsoft.Json;

namespace IsanPlugin
{
    [UsedImplicitly]
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
            var prefs = PluginUtils.GetPreferences(AssemblyInfo, () => new IsanPreferences());

            if (raw.V_ISAN != null && prefs.TryPopulate(raw.V_ISAN))
            {
                return;
            }

            Lookup(token, raw, derived);

            if (raw.V_ISAN != null)
            {
                prefs.Store(raw.V_ISAN);
                PluginUtils.SavePreferences(AssemblyInfo, prefs);
            }
        }

        private static void Lookup(ProgressToken token, DiscMetadata.RawMetadata raw, DiscMetadata.DerivedMetadata derived)
        {
            var provider = new IsanMetadataProvider(token);

            provider.Populate(raw.V_ISAN);

            var isan = raw.ISAN;
            if (isan != null && !string.IsNullOrWhiteSpace(isan.Title))
            {
                // TODO: Get language from isan.org
                // Don't insert twice
                if (!derived.SearchQueries.Any(query => query.Title == isan.Title && query.Year == isan.Year))
                    derived.SearchQueries.Insert(0, new SearchQuery { Title = isan.Title, Year = isan.Year });
            }
        }
    }

    internal class IsanPreferences
    {
        [JsonProperty(PropertyName = "cache")]
        public IDictionary<string, VIsanJson> Cache = new Dictionary<string, VIsanJson>();

        public bool TryPopulate([NotNull] VIsan vIsan)
        {
            if (!Cache.ContainsKey(vIsan.Number))
            {
                return false;
            }
            var json = Cache[vIsan.Number];
            json.Populate(vIsan);
            return true;
        }

        public void Store([NotNull] VIsan vIsan)
        {
            Cache[vIsan.Number] = vIsan.ToJson();
        }
    }
}
