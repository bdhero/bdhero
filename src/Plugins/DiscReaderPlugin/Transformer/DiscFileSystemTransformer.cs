using System;
using System.Collections.Generic;
using System.Drawing;
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
                            DBox = GetFile("FilmIndex.xml", bdrom.DirectoryRoot),
                            JacketImageSmall = null, /* assigned below */
                            JacketImageLarge = null /* assigned below */
                        }
                };

            fs.Directories.AACS = GetAACSDirectory(fs);
            fs.Directories.DCOPY = GetDCopyDirectory(fs);
            fs.Files.MCMF = GetFileOrBackup("mcmf.xml", fs.Directories.AACS);
            fs.Files.BDMT = GetFilesByPattern("bdmt_???.xml", fs.Directories.BDMT);
            fs.Files.JacketImageSmall = GetJacketImage(JacketSize.Small, fs.Directories.BDMT);
            fs.Files.JacketImageLarge = GetJacketImage(JacketSize.Large, fs.Directories.BDMT);

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

        [CanBeNull]
        private static FileInfo GetJacketImage(JacketSize jacketSize, DirectoryInfo dir)
        {
            var size = jacketSize.GetDimensions();
            var files = dir.GetFiles("*.jpg", SearchOption.AllDirectories)
                           .Where(info => Image.FromFile(info.FullName).Size.Equals(size))
                           .ToArray();
            return files.FirstOrDefault();
        }
    }

    // TODO: Externalize
    public enum JacketSize
    {
        /// <summary>
        ///     416 x 240
        /// </summary>
        [Size(416, 240)]
        Small,

        /// <summary>
        ///     640 x 360
        /// </summary>
        [Size(640, 360)]
        Large
    }

    // TODO: Externalize
    [AttributeUsage(AttributeTargets.Field)]
    public class SizeAttribute : Attribute
    {
        public Size Size { get; set; }

        public SizeAttribute(int width, int height)
        {
            Size = new Size(width, height);
        }
    }

    public static class JacketSizeExtensions
    {
        public static Size GetDimensions(this JacketSize jacketSize)
        {
            var type = typeof(JacketSize);
            var info = type.GetMember(jacketSize.ToString());

            if (!info.Any())
                return default(Size);

            var attr = info[0].GetCustomAttributes(typeof(SizeAttribute), false);

            if (!attr.Any())
                return default(Size);

            var description = ((SizeAttribute)attr[0]).Size;
            return description;
        }
    }
}
