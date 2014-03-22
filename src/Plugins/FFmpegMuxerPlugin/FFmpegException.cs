using System;

namespace BDHero.Plugin.FFmpegMuxer
{
    public class FFmpegException : Exception
    {
        public FFmpegException()
        {
        }

        public FFmpegException(string message)
            : base(message)
        {
        }

        public FFmpegException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
