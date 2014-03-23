using System;

namespace BDHero.Exceptions
{
    public class ID10TException : Exception
    {
        public ID10TException()
        {
        }

        public ID10TException(string message)
            : base(message)
        {
        }

        public ID10TException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
