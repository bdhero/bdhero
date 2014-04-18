using System;
using DotNetUtils.Exceptions;

namespace BDHero.Exceptions
{
    public class InvalidJobException : ID10TException
    {
        public InvalidJobException()
        {
        }

        public InvalidJobException(string message)
            : base(message)
        {
        }

        public InvalidJobException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
