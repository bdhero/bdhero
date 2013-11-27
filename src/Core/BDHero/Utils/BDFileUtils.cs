using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotNetUtils.Annotations;

namespace BDHero.Utils
{
    public static class BDFileUtils
    {
        [CanBeNull]
        public static string GetBDROMDirectory([CanBeNull] string path)
        {
            while (!string.IsNullOrWhiteSpace(path))
            {
                if (File.Exists(Path.Combine(path, "BDMV", "index.bdmv")))
                    return path;
                path = Path.GetDirectoryName(path);
            }

            return null;
        }

        public static bool IsBDROM(DriveInfo drive)
        {
            return IsBDROM(drive.Name);
        }

        public static bool IsBDROM([CanBeNull] string path)
        {
            return GetBDROMDirectory(path) != null;
        }
    }
}
