using System;

namespace TmdbPlugin
{
    public class TmdbException : Exception
    {
        public TmdbException()
        {
        }

        public TmdbException(string message) : base(message)
        {
        }

        public TmdbException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
