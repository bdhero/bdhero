using System;

namespace BDHero.Plugin.FFmpegMuxer
{
    internal class StdErrMessage
    {
        public DateTime TimeStamp;
        public string Message;

        public StdErrMessage()
        {
        }

        public StdErrMessage(DateTime timeStamp, string message)
        {
            TimeStamp = timeStamp;
            Message = message;
        }

        public override string ToString()
        {
            return string.Format("{0:yyyy-MM-dd HH:mm:ss,fff} {1}", TimeStamp, Message);
        }
    }
}