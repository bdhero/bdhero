//============================================================================
// BDInfo - Blu-ray Video and Audio Analysis Tool
// Copyright © 2010 Cinema Squid
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using I18N;
using WinAPI.Kernel;

namespace BDInfo
{
    public class BDROM
    {
        public DirectoryInfo DirectoryRoot = null;
        public DirectoryInfo DirectoryBDMV = null;
        public DirectoryInfo DirectoryBDJO = null;
        public DirectoryInfo DirectoryCLIPINF = null;
        public DirectoryInfo DirectoryPLAYLIST = null;
        public DirectoryInfo DirectorySNP = null;
        public DirectoryInfo DirectorySSIF = null;
        public DirectoryInfo DirectorySTREAM = null;

        /// <summary>
        /// Auto-detected main audience language of the disc.
        /// Found by checking for the existence of META\DL\bdmt_[lang].xml for all languages.
        /// </summary>
        public Language DiscLanguage = null;
        public string DiscName = null;
        public string VolumeLabel = null;
        public ulong Size = 0;
        public int MainTitleIndex = -1;
        public bool IsBDPlus = false;
        public bool IsBDJava = false;
        public bool IsDBOX = false;
        public bool IsPSP = false;
        public bool Is3D = false;
        public bool Is50Hz = false;

        public string DiscNameSearchable
        {
            get
            {
                string movieName = DiscName;
                if (movieName != null)
                {
                    movieName = Regex.Replace(movieName, @" - Blu-ray.*", "", RegexOptions.IgnoreCase);
                    movieName = Regex.Replace(movieName, @" \(?Disc \d+\)?", "", RegexOptions.IgnoreCase);
                    movieName = Regex.Replace(movieName, @"\s*[[(].*", "", RegexOptions.IgnoreCase);
                    movieName = movieName.Trim();
                }
                return movieName;
            }
        }

        public Dictionary<string, TSPlaylistFile> PlaylistFiles = 
            new Dictionary<string, TSPlaylistFile>();
        public Dictionary<string, TSStreamClipFile> StreamClipFiles =
            new Dictionary<string, TSStreamClipFile>();
        public Dictionary<string, TSStreamFile> StreamFiles = 
            new Dictionary<string, TSStreamFile>();
        public Dictionary<string, TSInterleavedFile> InterleavedFiles =
            new Dictionary<string, TSInterleavedFile>();

        private static List<string> ExcludeDirs = new List<string> { "ANY!", "AACS", "BDSVM", "ANYVM", "SLYVM" };

        public delegate bool OnStreamClipFileScanError(
            TSStreamClipFile streamClipFile, Exception ex);

        public event OnStreamClipFileScanError StreamClipFileScanError;

        public delegate bool OnStreamFileScanError(
            TSStreamFile streamClipFile, Exception ex);

        public event OnStreamFileScanError StreamFileScanError;

        public delegate bool OnPlaylistFileScanError(
            TSPlaylistFile playlistFile, Exception ex);

        public event OnPlaylistFileScanError PlaylistFileScanError;

        public BDROM(
            string path)
        {
            //
            // Locate BDMV directories.
            //

            DirectoryBDMV = 
                GetDirectoryBDMV(path);
            
            if (DirectoryBDMV == null)
            {
                throw new IOException("Unable to locate BD structure.");
            }

            DirectoryRoot = 
                DirectoryBDMV.Parent;
            DirectoryBDJO = 
                GetDirectory("BDJO", DirectoryBDMV, 0);
            DirectoryCLIPINF = 
                GetDirectory("CLIPINF", DirectoryBDMV, 0);
            DirectoryPLAYLIST =
                GetDirectory("PLAYLIST", DirectoryBDMV, 0);
            DirectorySNP =
                GetDirectory("SNP", DirectoryRoot, 0);
            DirectorySTREAM = 
                GetDirectory("STREAM", DirectoryBDMV, 0);
            DirectorySSIF =
                GetDirectory("SSIF", DirectorySTREAM, 0);

            if (DirectoryCLIPINF == null
                || DirectoryPLAYLIST == null)
            {
                throw new IOException("Unable to locate BD structure.");
            }

            //
            // Initialize basic disc properties.
            //

            DiscName = GetDiscName();
            VolumeLabel = GetVolumeLabel(DirectoryRoot);
            Size = (ulong)GetDirectorySize(DirectoryRoot);
            
            if (null != GetDirectory("BDSVM", DirectoryRoot, 0))
            {
                IsBDPlus = true;
            }
            if (null != GetDirectory("SLYVM", DirectoryRoot, 0))
            {
                IsBDPlus = true;
            }
            if (null != GetDirectory("ANYVM", DirectoryRoot, 0))
            {
                IsBDPlus = true;
            }

            if (DirectoryBDJO != null &&
                DirectoryBDJO.GetFiles().Length > 0)
            {
                IsBDJava = true;
            }

            if (DirectorySNP != null &&
                (DirectorySNP.GetFiles("*.mnv").Length > 0 || DirectorySNP.GetFiles("*.MNV").Length > 0))
            {
                IsPSP = true;
            }

            if (DirectorySSIF != null &&
                DirectorySSIF.GetFiles().Length > 0)
            {
                Is3D = true;
            }

            if (File.Exists(Path.Combine(DirectoryRoot.FullName, "FilmIndex.xml")))
            {
                IsDBOX = true;
            }

            //
            // Initialize file lists.
            //

            if (DirectoryPLAYLIST != null)
            {
                FileInfo[] files = DirectoryPLAYLIST.GetFiles("*.mpls");
                if (files.Length == 0)
                {
                    files = DirectoryPLAYLIST.GetFiles("*.MPLS");
                }
                foreach (FileInfo file in files)
                {
                    PlaylistFiles.Add(
                        file.Name.ToUpper(), new TSPlaylistFile(this, file));
                }
            }

            if (DirectorySTREAM != null)
            {
                FileInfo[] files = DirectorySTREAM.GetFiles("*.m2ts");
                if (files.Length == 0)
                {
                    files = DirectoryPLAYLIST.GetFiles("*.M2TS");
                }
                foreach (FileInfo file in files)
                {
                    StreamFiles.Add(
                        file.Name.ToUpper(), new TSStreamFile(file));
                }
            }

            if (DirectoryCLIPINF != null)
            {
                FileInfo[] files = DirectoryCLIPINF.GetFiles("*.clpi");
                if (files.Length == 0)
                {
                    files = DirectoryPLAYLIST.GetFiles("*.CLPI");
                }
                foreach (FileInfo file in files)
                {
                    StreamClipFiles.Add(
                        file.Name.ToUpper(), new TSStreamClipFile(file));
                }
            }

            if (DirectorySSIF != null)
            {
                FileInfo[] files = DirectorySSIF.GetFiles("*.ssif");
                if (files.Length == 0)
                {
                    files = DirectorySSIF.GetFiles("*.SSIF");
                }
                foreach (FileInfo file in files)
                {
                    InterleavedFiles.Add(
                        file.Name.ToUpper(), new TSInterleavedFile(file));
                }
            }
        }

        private void ReportScanProgress(string fileType, string fileName, int curStep,
            int curFileOfType, int numFilesOfType, int completedFilesOfType,
            int curFileOverall, int numFilesOverall, int completedFilesOverall)
        {
            if (ScanProgress != null)
                ScanProgress(new BDROMScanProgressState
                                        {
                                            FileType = fileType,
                                            FileName = fileName,
                                            CurStep = curStep,
                                            NumSteps = 3,
                                            CurFileOfType = curFileOfType,
                                            NumFilesOfType = numFilesOfType,
                                            CompletedFilesOfType = completedFilesOfType,
                                            CurFileOverall = curFileOverall,
                                            NumFilesOverall = numFilesOverall,
                                            CompletedFilesOverall = completedFilesOverall
                                        });
        }

        public event BDROMScanProgressHandler ScanProgress;

        public BDROM Scan(CancellationToken cancellationToken)
        {
            var numStreamClipFiles = StreamClipFiles.Count;
            var numPlaylists = PlaylistFiles.Count;
            var numStreamFiles = StreamFiles.Count;
            var numFilesOverall = numStreamClipFiles + numPlaylists + numStreamFiles;

            var curStreamClipFile = 0;
            var curPlaylist = 0;
            var curStreamFile = 0;
            var curFileOverall = 0;

            var completedStreamClipFiles = 0;
            var completedPlaylists = 0;
            var completedStreamFiles = 0;
            var completedFilesOverall = 0;

            List<TSStreamClipFile> errorStreamClipFiles = new List<TSStreamClipFile>();
            foreach (TSStreamClipFile streamClipFile in StreamClipFiles.Values)
            {
                if (cancellationToken.IsCancellationRequested)
                    return this;

                try
                {
                    ReportScanProgress("stream clip", streamClipFile.Name, 1,
                        ++curStreamClipFile, numStreamClipFiles, completedStreamClipFiles,
                        ++curFileOverall, numFilesOverall, completedFilesOverall);
                    streamClipFile.Scan();
                    ReportScanProgress("stream clip", streamClipFile.Name, 1,
                        curStreamClipFile, numStreamClipFiles, ++completedStreamClipFiles,
                        curFileOverall, numFilesOverall, ++completedFilesOverall);
                }
                catch (Exception ex)
                {
                    errorStreamClipFiles.Add(streamClipFile);
                    if (StreamClipFileScanError != null)
                    {
                        if (StreamClipFileScanError(streamClipFile, ex))
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else throw;
                }
            }

            // TODO: Report progress
            foreach (TSStreamFile streamFile in StreamFiles.Values)
            {
                string ssifName = Path.GetFileNameWithoutExtension(streamFile.Name) + ".SSIF";
                if (InterleavedFiles.ContainsKey(ssifName))
                {
                    streamFile.InterleavedFile = InterleavedFiles[ssifName];
                }
            }

            TSStreamFile[] streamFiles = new TSStreamFile[StreamFiles.Count];
            StreamFiles.Values.CopyTo(streamFiles, 0);
            Array.Sort(streamFiles, CompareStreamFiles);

            List<TSPlaylistFile> errorPlaylistFiles = new List<TSPlaylistFile>();
            foreach (TSPlaylistFile playlistFile in PlaylistFiles.Values)
            {
                if (cancellationToken.IsCancellationRequested)
                    return this;

                try
                {
                    ReportScanProgress("playlist", playlistFile.Name, 2,
                        ++curPlaylist, numPlaylists, completedPlaylists,
                        ++curFileOverall, numFilesOverall, completedFilesOverall);
                    playlistFile.Scan(StreamFiles, StreamClipFiles);
                    ReportScanProgress("playlist", playlistFile.Name, 2,
                        curPlaylist, numPlaylists, ++completedPlaylists,
                        curFileOverall, numFilesOverall, ++completedFilesOverall);
                }
                catch (Exception ex)
                {
                    errorPlaylistFiles.Add(playlistFile);
                    if (PlaylistFileScanError != null)
                    {
                        if (PlaylistFileScanError(playlistFile, ex))
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else throw;
                }
            }

            List<TSStreamFile> errorStreamFiles = new List<TSStreamFile>();
            foreach (TSStreamFile streamFile in streamFiles)
            {
                if (cancellationToken.IsCancellationRequested)
                    return this;

                try
                {
                    List<TSPlaylistFile> playlists = new List<TSPlaylistFile>();
                    foreach (TSPlaylistFile playlist in PlaylistFiles.Values)
                    {
                        foreach (TSStreamClip streamClip in playlist.StreamClips)
                        {
                            if (streamClip.Name == streamFile.Name)
                            {
                                playlists.Add(playlist);
                                break;
                            }
                        }
                    }
                    ReportScanProgress("stream", streamFile.Name, 3,
                        ++curStreamFile, numStreamFiles, completedStreamFiles,
                        ++curFileOverall, numFilesOverall, completedFilesOverall);
                    streamFile.Scan(playlists, false);
                    ReportScanProgress("stream", streamFile.Name, 3,
                        curStreamFile, numStreamFiles, ++completedStreamFiles,
                        curFileOverall, numFilesOverall, ++completedFilesOverall);
                }
                catch (Exception ex)
                {
                    errorStreamFiles.Add(streamFile);
                    if (StreamFileScanError != null)
                    {
                        if (StreamFileScanError(streamFile, ex))
                        {
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else throw;
                }
            }

            foreach (TSPlaylistFile playlistFile in PlaylistFiles.Values)
            {
                if (cancellationToken.IsCancellationRequested)
                    return this;

                playlistFile.Initialize();
                if (!Is50Hz)
                {
                    foreach (TSVideoStream videoStream in playlistFile.VideoStreams)
                    {
                        if (videoStream.FrameRate == TSFrameRate.FRAMERATE_25 ||
                            videoStream.FrameRate == TSFrameRate.FRAMERATE_50)
                        {
                            Is50Hz = true;
                        }
                    }
                }
            }

            return this;
        }

        private DirectoryInfo GetDirectoryBDMV(
            string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            while (dir != null)
            {
                if (dir.Name == "BDMV")
                {
                    return dir;
                }
                dir = dir.Parent;
            }

            return GetDirectory("BDMV", new DirectoryInfo(path), 0);
        }

        private DirectoryInfo GetDirectory(
            string name,
            DirectoryInfo dir,
            int searchDepth)
        {
            if (dir != null)
            {
                DirectoryInfo[] children = dir.GetDirectories();
                foreach (DirectoryInfo child in children)
                {
                    if (child.Name == name)
                    {
                        return child;
                    }
                }
                if (searchDepth > 0)
                {
                    foreach (DirectoryInfo child in children)
                    {
                        GetDirectory(
                            name, child, searchDepth - 1);
                    }
                }
            }
            return null;
        }

        private long GetDirectorySize(DirectoryInfo directoryInfo)
        {
            long size = 0;

            //if (!ExcludeDirs.Contains(directoryInfo.Name.ToUpper()))  // TODO: Keep?
            {
                FileInfo[] pathFiles = directoryInfo.GetFiles();
                foreach (FileInfo pathFile in pathFiles)
                {
                    if (pathFile.Extension.ToUpper() == ".SSIF")
                    {
                        continue;
                    }
                    size += pathFile.Length;
                }

                DirectoryInfo[] pathChildren = directoryInfo.GetDirectories();
                foreach (DirectoryInfo pathChild in pathChildren)
                {
                    size += GetDirectorySize(pathChild);
                }
            }

            return size;
        }

        // TODO: Fix this garbage
        private string GetBdmtPath()
        {
            var paths = new Dictionary<string, string>();
            foreach (string code in Language.ISO6392Codes)
            {
                string path = Path.Combine(DirectoryBDMV.FullName, @"META\DL\bdmt_" + code + @".xml");
                if (File.Exists(path))
                {
                    paths[code] = path;
                }
            }
            if (paths.Count > 1)
            {
                string iso6392;

                iso6392 = Language.CurrentUILanguage.ISO_639_2;
                if (paths.ContainsKey(iso6392))
                {
                    DiscLanguage = Language.FromCode(iso6392);
                    return paths[iso6392];
                }

                iso6392 = Language.English.ISO_639_2;
                if (paths.ContainsKey(iso6392))
                {
                    DiscLanguage = Language.FromCode(iso6392);
                    return paths[iso6392];
                }
            }
            if (paths.Count >= 1)
            {
                var code = paths.Keys.OrderBy(s => s).First();
                var path = paths[code];
                DiscLanguage = Language.FromCode(code);
                return path;
            }
            return null;
        }

        private string GetDiscName()
        {
            string discName = GetDiscNameBdmt();
            if (discName == null)
            {
                discName = GetDiscNameFileIndex();
            }
            return discName;
        }

        private string GetDiscNameBdmt()
        {
            string xmlpath = GetBdmtPath();
            string discName = null;

            if (xmlpath != null)
            {
                Regex movieNameRegex = new Regex("<di:name>(.*?)</di:name>", RegexOptions.IgnoreCase);
                string xml = File.ReadAllText(xmlpath);

                if (movieNameRegex.IsMatch(xml))
                {
                    Match movieNameMatch = movieNameRegex.Match(xml);

                    discName = movieNameMatch.Groups[1].ToString();

                    Regex titleRegex = new Regex("<di:titleName titleNumber=\"(\\d+)\">(.*?)</di:titleName>");

                    if (titleRegex.IsMatch(xml))
                    {
                        MatchCollection titleMatches = titleRegex.Matches(xml);

                        foreach (Match titleMatch in titleMatches)
                        {
                            if (titleMatch.Groups[2].ToString().Equals(discName))
                            {
                                MainTitleIndex = int.Parse(titleMatch.Groups[1].ToString()) - 1;
                                break;
                            }
                        }
                    }
                }
            }

            return discName;
        }

        private string GetDiscNameFileIndex()
        {
            string xmlpath = Path.Combine(DirectoryRoot.FullName, "FilmIndex.xml");
            string discName = null;

            if (File.Exists(xmlpath))
            {
                Regex movieNameRegex = new Regex("<Title>(.*?)</Title>", RegexOptions.IgnoreCase);
                string xml = File.ReadAllText(xmlpath);

                if (movieNameRegex.IsMatch(xml))
                {
                    Match movieNameMatch = movieNameRegex.Match(xml);
                    discName = movieNameMatch.Groups[1].ToString();
                }
            }

            return discName;
        }

        private static string GetVolumeLabel(DirectoryInfo dir)
        {
            var volume = VolumeAPI.GetVolumeInformation(dir);
            var isRoot = dir.FullName == dir.Root.FullName;
            if (isRoot && !string.IsNullOrEmpty(volume.Label))
                return volume.Label;
            return dir.Name;
        }

        public static int CompareStreamFiles(
            TSStreamFile x,
            TSStreamFile y)
        {
            // TODO: Use interleaved file sizes

            if ((x == null || x.FileInfo == null) && (y == null || y.FileInfo == null))
            {
                return 0;
            }
            else if ((x == null || x.FileInfo == null) && (y != null && y.FileInfo != null))
            {
                return 1;
            }
            else if ((x != null || x.FileInfo != null) && (y == null || y.FileInfo == null))
            {
                return -1;
            }
            else
            {
                if (x.FileInfo.Length > y.FileInfo.Length)
                {
                    return 1;
                }
                else if (y.FileInfo.Length > x.FileInfo.Length)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public class BDROMScanProgressState
    {
        /// <summary>
        /// "stream clip", "playlist", or "stream"
        /// </summary>
        public string FileType;

        /// <summary>
        /// "00850.MPLS", "00100.CLPI", "00400.M2TS"
        /// </summary>
        public string FileName;

        public int CurStep;
        public int NumSteps;

        public int CurFileOfType;
        public int NumFilesOfType;
        public int CompletedFilesOfType;

        /// <summary>
        /// 0.0 to 100.0
        /// </summary>
        public double TypeProgress
        {
            get { return 100 * ((double) CompletedFilesOfType)/NumFilesOfType; }
        }

        public int CurFileOverall;
        public int NumFilesOverall;
        public int CompletedFilesOverall;

        /// <summary>
        /// 0.0 to 100.0
        /// </summary>
        public double OverallProgress
        {
            get { return 100 * ((double)CompletedFilesOverall) / NumFilesOverall; }
        }
    }

    public delegate void BDROMScanProgressHandler(BDROMScanProgressState state);
}
