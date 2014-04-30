using System;
using DotNetUtils.Exceptions;

namespace BDHero.Plugin.MkvMergeMuxer
{
    public class MkvMergeException : ReportableException
    {
        public MkvMergeException()
        {
        }

        public MkvMergeException(string message)
            : base(message)
        {
        }

        public MkvMergeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
