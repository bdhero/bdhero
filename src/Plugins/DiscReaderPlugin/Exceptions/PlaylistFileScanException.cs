using System;
using BDInfo;

namespace BDHero.Plugin.DiscReader.Exceptions
{
    public class PlaylistFileScanException : ScanException
    {
        public TSPlaylistFile PlaylistFile;

        public PlaylistFileScanException()
        {
        }

        public PlaylistFileScanException(string message)
            : base(message)
        {
        }

        public PlaylistFileScanException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}