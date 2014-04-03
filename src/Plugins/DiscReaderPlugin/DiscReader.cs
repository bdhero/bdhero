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
using System.Drawing;
using System.IO;
using System.Threading;
using BDHero.BDROM;
using BDHero.Exceptions;
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

            var bdrom = ScanBDROM(cancellationToken, bdromPath);

            if (cancellationToken.IsCancellationRequested)
                return null;

            Host.ReportProgress(this, 99.0, "Transforming BDInfo => BDHero...", "Transforming BDInfo structure into BDHero structure...");

            var disc = DiscTransformer.Transform(bdrom);

            Host.ReportProgress(this, 100.0, "Finished scanning BD-ROM");

            return disc;
        }

        private BDInfo.BDROM ScanBDROM(CancellationToken cancellationToken, string bdromPath)
        {
            try
            {
                var bdrom = new BDInfo.BDROM(bdromPath);
                bdrom.ScanProgress += BDROMOnScanProgress;
                bdrom.Scan(cancellationToken);
                return bdrom;
            }
            catch (Exception e)
            {
                if (e is ArgumentException ||
                    e is IOException)
                {
                    throw new ID10TException(e.Message, e);
                }
                throw;
            }
        }

        private void BDROMOnScanProgress(BDROMScanProgressState bdromState)
        {
#if false
            Console.WriteLine("BDROM: {0}: scanning {1} of {2} ({3}%).  Total: {4} of {5} ({6}%).",
                bdromState.FileType, bdromState.CurFileOfType, bdromState.NumFilesOfType, bdromState.TypeProgress.ToString("0.00"),
                bdromState.CurFileOverall, bdromState.NumFilesOverall, bdromState.OverallProgress.ToString("0.00"));
#endif
            Host.ReportProgress(this, bdromState.OverallProgress * .99,
                                string.Format("{1} ({0})", bdromState.FileType, bdromState.FileName),
                                string.Format("Scanning {0} file {1}", bdromState.FileType, bdromState.FileName));
        }
    }
}
