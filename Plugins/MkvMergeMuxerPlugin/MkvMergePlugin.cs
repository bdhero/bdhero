using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using BDHero.JobQueue;

namespace BDHero.Plugin.MkvMergeMuxer
{
    public class MkvMergePlugin : IMuxerPlugin
    {
        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "mkvmerge (mkvtoolnix)"; } }

        public bool Enabled { get; set; }

        public Icon Icon { get { return Resources.mkvmerge_icon; } }

        public int RunOrder { get { return 1; } }

        public EditPluginPreferenceHandler EditPreferences { get; private set; }

        public MatroskaFeatures SupportedFeatures
        {
            get
            {
                return MatroskaFeatures.Chapters
                     | MatroskaFeatures.CoverArt
                     | MatroskaFeatures.DefaultFlag
                     | MatroskaFeatures.ForcedFlag
                    ;
            }
        }

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        public void Mux(CancellationToken cancellationToken, Job job)
        {
        }
    }
}
