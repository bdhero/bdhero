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

#undef DEBUG_SCAN_ERRORS

using System;
using System.Drawing;
using System.IO;
using System.Threading;
using BDHero.BDROM;
using BDHero.Plugin.DiscReader.Exceptions;
using BDHero.Plugin.DiscReader.Transformer;
using BDInfo;
using DotNetUtils.Annotations;
using DotNetUtils.Exceptions;

namespace BDHero.Plugin.DiscReader
{
    [UsedImplicitly]
    public class DiscReader : IDiscReaderPlugin
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                bdrom.PlaylistFileScanError += BDROMOnPlaylistFileScanError;
                bdrom.StreamFileScanError += BDROMOnStreamFileScanError;
                bdrom.StreamClipFileScanError += BDROMOnStreamClipFileScanError;
                bdrom.Scan(cancellationToken);
                return bdrom;
            }
            catch (Exception e)
            {
                if (e is ArgumentException ||
                    e is UnauthorizedAccessException ||
                    e is IOException)
                {
                    throw new ID10TException(e.Message, e);
                }
                throw;
            }
        }

        private void BDROMOnScanProgress(BDROMScanProgressState bdromState)
        {
            Host.ReportProgress(this, bdromState.OverallProgress * .99,
                                string.Format("{1} ({0})", bdromState.FileType, bdromState.FileName),
                                string.Format("Scanning {0} file {1}", bdromState.FileType, bdromState.FileName));
        }

        private bool BDROMOnPlaylistFileScanError(TSPlaylistFile playlistFile, Exception exception)
        {
            var message = string.Format("Error occurred while scanning playlist file {0}", playlistFile.Name);
#if DEBUG_SCAN_ERRORS
            Logger.Warn(message, exception);
            return true;
#else
            throw new PlaylistFileScanException(message, exception) { PlaylistFile = playlistFile };
#endif
        }

        private bool BDROMOnStreamFileScanError(TSStreamFile streamFile, Exception exception)
        {
            var message = string.Format("Error occurred while scanning stream file {0}", streamFile.Name);
#if DEBUG_SCAN_ERRORS
            Logger.Warn(message, exception);
            return true;
#else
            throw new StreamFileScanException(message, exception) { StreamFile = streamFile };
#endif
        }

        private bool BDROMOnStreamClipFileScanError(TSStreamClipFile streamClipFile, Exception exception)
        {
            var message = string.Format("Error occurred while scanning stream clip file {0}", streamClipFile.Name);
#if DEBUG_SCAN_ERRORS
            Logger.Warn(message, exception);
            return true;
#else
            throw new StreamClipFileScanException(message, exception) { StreamClipFile = streamClipFile };
#endif
        }
    }
}
