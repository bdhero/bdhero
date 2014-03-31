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
using System.Linq;
using DotNetUtils.Annotations;
using OSUtils;
using OSUtils.Info;

namespace UpdateLib
{
    public class Update
    {
        public readonly Version Version;
        public readonly string FileName;
        public readonly string Uri;
        public readonly string SHA1;
        public readonly long Size;

        public Update(Version version, string fileName, string uri, string sha1, long size)
        {
            Version = version;
            FileName = fileName;
            Uri = uri;
            SHA1 = sha1;
            Size = size;
        }

        #region Response conversion

        [CanBeNull]
        public static Update FromResponse([NotNull] UpdateResponse response, bool isPortable)
        {
            var mirror = response.Mirrors.First();
            var platform = GetPlatform(response);
            var package = GetPackage(platform, isPortable);

            // No package available for the user's OS
            if (package == null)
            {
                return null;
            }

            var version = response.Version;
            var filename = package.FileName;
            var uri = mirror + filename;

            return new Update(version, filename, uri, package.SHA1, package.Size);
        }

        [NotNull]
        private static Platform GetPlatform([NotNull] UpdateResponse response)
        {
            var platforms = response.Platforms;
            var osType = SystemInfo.Instance.OS.Type;
            if (OSType.Mac == osType)
                return platforms.Mac;
            if (OSType.Linux == osType)
                return platforms.Linux;
            return platforms.Windows;
        }

        [CanBeNull]
        private static Package GetPackage([NotNull] Platform platform, bool isPortable)
        {
            return isPortable ? platform.Packages.Portable : platform.Packages.Setup;
        }

        #endregion
    }
}