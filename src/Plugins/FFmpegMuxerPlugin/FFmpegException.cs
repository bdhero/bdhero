using System;
using DotNetUtils.Exceptions;

namespace BDHero.Plugin.FFmpegMuxer
{
    public class FFmpegException : ReportableException
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
