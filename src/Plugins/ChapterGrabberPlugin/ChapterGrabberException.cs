using System;

namespace ChapterGrabberPlugin
{
    public class ChapterGrabberException : Exception
    {
        public ChapterGrabberException()
        {
        }

        public ChapterGrabberException(string message) : base(message)
        {
        }

        public ChapterGrabberException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
