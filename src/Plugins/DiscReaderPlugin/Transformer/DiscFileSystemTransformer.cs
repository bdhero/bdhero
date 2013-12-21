﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using DotNetUtils.Annotations;

namespace BDHero.Plugin.DiscReader.Transformer
{
    static class DiscFileSystemTransformer
    {
        public static void Transform(BDInfo.BDROM bdrom, Disc disc)
        {
            var fs = new DiscFileSystem
                {
                    Directories = new DiscFileSystem.DiscDirectories
                        {
                            Root = bdrom.DirectoryRoot,
                            BDMV = bdrom.DirectoryBDMV,
                            CLIPINF = bdrom.DirectoryCLIPINF,
                            PLAYLIST = bdrom.DirectoryPLAYLIST,
                            STREAM = bdrom.DirectorySTREAM,
                            SSIF = bdrom.DirectorySSIF,
                            BDMT = GetBDMTDirectory(bdrom.DirectoryBDMV),
                            BDJO = bdrom.DirectoryBDJO,
                            SNP = bdrom.DirectorySNP,
                            ANY = GetDirectory("ANY!", bdrom.DirectoryRoot),
                            MAKEMKV = GetDirectory("MAKEMKV", bdrom.DirectoryRoot),
                            AACS = null /* assigned below */,
                            DCOPY = null /* assigned below */
                        },
                    Files = new DiscFileSystem.DiscFiles
                        {
                            AnyDVDDiscInf = GetFile("disc.inf", bdrom.DirectoryRoot),
                            MCMF = null, /* assigned below */
                            BDMT = new FileInfo[0], /* assigned below */
                            DBox = GetFile("FilmIndex.xml", bdrom.DirectoryRoot)
                        }
                };

            fs.Directories.AACS = GetAACSDirectory(fs);
            fs.Directories.DCOPY = GetDCopyDirectory(fs);
            fs.Files.MCMF = GetFileOrBackup("mcmf.xml", fs.Directories.AACS);
            fs.Files.BDMT = GetFilesByPattern("bdmt_???.xml", fs.Directories.BDMT);

            disc.FileSystem = fs;
        }

        [CanBeNull]
        private static DirectoryInfo GetBDMTDirectory(DirectoryInfo bdmv)
        {
            var path = Path.Combine(bdmv.FullName, "META", "DL");
            return Directory.Exists(path) ? new DirectoryInfo(path) : null;
        }

        private static DirectoryInfo GetAACSDirectory(DiscFileSystem fs)
        {
            return fs.Directories.ANY ??
                   GetDirectory("AACS", fs.Directories.MAKEMKV) ??
                   GetDirectory("AACS", fs.Directories.Root);
        }

        private static DirectoryInfo GetDCopyDirectory(DiscFileSystem fs)
        {
            var dir = new DirectoryInfo(Path.Combine(fs.Directories.Root.FullName, "DCOPY"));
            return dir.Exists ? dir : null;
        }

        [CanBeNull]
        private static DirectoryInfo GetDirectory(string name, DirectoryInfo dir)
        {
            return dir != null ? dir.GetDirectories().FirstOrDefault(info => info.Name == name) : null;
        }

        [CanBeNull]
        private static FileInfo GetFile(string name, DirectoryInfo dir)
        {
            return dir != null ? dir.GetFiles().FirstOrDefault(info => info.Name == name) : null;
        }

        [CanBeNull]
        private static FileInfo GetFileOrBackup(string name, DirectoryInfo dir)
        {
            if (dir == null)
                return null;
            var file = dir.GetFiles().FirstOrDefault(info => info.Name == name);
            if (file != null)
                return file;
            var duplicateDir = dir.GetDirectories().FirstOrDefault(info => "DUPLICATE" == info.Name.ToUpperInvariant());
            return GetFile(name, duplicateDir);
        }

        [NotNull]
        private static FileInfo[] GetFilesByPattern(string pattern, DirectoryInfo dir)
        {
            return dir != null ? dir.GetFiles(pattern) : new FileInfo[0];
        }
    }
}
