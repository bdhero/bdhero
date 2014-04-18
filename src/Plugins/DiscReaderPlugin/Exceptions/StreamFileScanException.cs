using System;
using BDInfo;

namespace BDHero.Plugin.DiscReader.Exceptions
{
    public class StreamFileScanException : ScanException
    {
        public TSStreamFile StreamFile;

        public StreamFileScanException()
        {
        }

        public StreamFileScanException(string message)
            : base(message)
        {
        }

        public StreamFileScanException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}