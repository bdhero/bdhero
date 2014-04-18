using System;
using BDInfo;

namespace BDHero.Plugin.DiscReader.Exceptions
{
    public class StreamClipFileScanException : ScanException
    {
        public TSStreamClipFile StreamClipFile;

        public StreamClipFileScanException()
        {
        }

        public StreamClipFileScanException(string message)
            : base(message)
        {
        }

        public StreamClipFileScanException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}