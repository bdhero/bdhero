using System;

namespace BDHero.Plugin.DiscReader.Exceptions
{
    public class DiscMetadataTransformerException : ScanException
    {
        public DiscMetadataTransformerException()
        {
        }

        public DiscMetadataTransformerException(string message)
            : base(message)
        {
        }

        public DiscMetadataTransformerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
