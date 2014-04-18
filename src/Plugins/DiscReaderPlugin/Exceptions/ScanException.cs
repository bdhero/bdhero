using System;
using DotNetUtils.Exceptions;

namespace BDHero.Plugin.DiscReader.Exceptions
{
    public abstract class ScanException : ReportableException
    {
        protected ScanException()
        {
        }

        protected ScanException(string message)
            : base(message)
        {
        }

        protected ScanException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
