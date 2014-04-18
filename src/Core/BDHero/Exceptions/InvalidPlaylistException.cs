using System;
using DotNetUtils.Exceptions;

namespace BDHero.Exceptions
{
    public class InvalidPlaylistException : ID10TException
    {
        public InvalidPlaylistException()
        {
        }

        public InvalidPlaylistException(string message)
            : base(message)
        {
        }

        public InvalidPlaylistException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
