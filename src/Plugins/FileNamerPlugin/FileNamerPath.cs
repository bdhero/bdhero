using System.IO;

namespace BDHero.Plugin.FileNamer
{
    internal class FileNamerPath
    {
        public string Directory;
        public string FileName;

        public string FullName { get { return Path.Combine(Directory, FileName); } }

        public FileNamerPath(string directory, string fileName)
        {
            Directory = directory;
            FileName = fileName;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}