using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using BDHero.BDROM;
using BDHero.Plugin.DiscReader.Transformer;
using BDInfo;

namespace BDHero.Plugin.DiscReader
{
    public class DiscReader : IDiscReaderPlugin
    {
        public IPluginHost Host { get; private set; }

        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "BDInfo Disc Reader"; } }

        public bool Enabled { get; set; }

        public Icon Icon { get { return Resources.cinemasquid_icon; } }

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

        public Disc ReadBDROM(CancellationToken cancellationToken, string bdromPath)
        {
            if (cancellationToken.IsCancellationRequested)
                return null;

            Host.ReportProgress(this, 0.0, "Scanning BD-ROM...");

            var bdrom = new BDInfo.BDROM(bdromPath);
            bdrom.ScanProgress += BDROMOnScanProgress;
            bdrom.Scan(cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return null;

            Host.ReportProgress(this, 99.0, "Transforming BDInfo structure into BDHero structure...");

            var disc = DiscTransformer.Transform(bdrom);

            Host.ReportProgress(this, 100.0, "Finished scanning BD-ROM");

            return disc;
        }

        private void BDROMOnScanProgress(BDROMScanProgressState bdromState)
        {
#if false
            Console.WriteLine("BDROM: {0}: scanning {1} of {2} ({3}%).  Total: {4} of {5} ({6}%).",
                bdromState.FileType, bdromState.CurFileOfType, bdromState.NumFilesOfType, bdromState.TypeProgress.ToString("0.00"),
                bdromState.CurFileOverall, bdromState.NumFilesOverall, bdromState.OverallProgress.ToString("0.00"));
#endif
            Host.ReportProgress(this, bdromState.OverallProgress * .99,
                                string.Format("Scanning {0} file {1}", bdromState.FileType, bdromState.FileName));
        }
    }
}
