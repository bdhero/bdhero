using System;

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
    }
}