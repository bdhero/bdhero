// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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
using System.IO;
using System.Linq;
using System.Text;
using BDHero.BDROM;

namespace BDHero.Plugin.DiscReader.Transformer
{
    static class DiscFeaturesTransformer
    {
        public static void Transform(Disc disc)
        {
            var fs = disc.FileSystem;
            var root = fs.Directories.Root;
            var features = new DiscFeatures
                {
                    IsBDPlus = IsBDPlus(root),
                    IsBDJava = IsBDJava(fs),
                    Is3D = Is3D(fs),
                    IsDbox = IsDbox(fs),
                    IsDCopy = IsDCopy(fs),
                    IsPSP = IsPSP(fs)
                };
            disc.Features = features;
        }

        private static bool IsBDPlus(DirectoryInfo root)
        {
            return null != GetDirectory("BDSVM", root) ||
                   null != GetDirectory("SLYVM", root) ||
                   null != GetDirectory("ANYVM", root);
        }

        private static bool IsBDJava(DiscFileSystem fs)
        {
            return HasFiles(fs.Directories.BDJO);
        }

        private static bool Is3D(DiscFileSystem fs)
        {
            return HasFiles(fs.Directories.SSIF);
        }

        private static bool IsDbox(DiscFileSystem fs)
        {
            return File.Exists(Path.Combine(fs.Directories.Root.FullName, "FilmIndex.xml"));
        }

        private static bool IsDCopy(DiscFileSystem fs)
        {
            return HasFiles(fs.Directories.DCOPY);
        }

        private static bool IsPSP(DiscFileSystem fs)
        {
            return fs.Directories.SNP != null &&
                  (HasFiles(fs.Directories.SNP, "*.mnv") || HasFiles(fs.Directories.SNP, "*.MNV"));
        }

        private static bool HasFiles(DirectoryInfo directory)
        {
            return directory != null && directory.GetFiles().Any();
        }

        private static bool HasFiles(DirectoryInfo directory, string pattern)
        {
            return directory != null && directory.GetFiles(pattern).Any();
        }

        private static DirectoryInfo GetDirectory(string name, DirectoryInfo dir)
        {
            return dir != null ? dir.GetDirectories().FirstOrDefault(info => info.Name == name) : null;
        }
    }
}
