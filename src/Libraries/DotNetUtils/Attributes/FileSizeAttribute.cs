using System;
using DotNetUtils.FS;

namespace DotNetUtils.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class FileSizeAttribute : Attribute
    {
        public string Format(long numBytes)
        {
            var human = FileUtils.HumanFriendlyFileSize(numBytes);
            return string.Format("{0} bytes ({1})", numBytes, human);
        }
        public string Format(ulong numBytes)
        {
            var human = FileUtils.HumanFriendlyFileSize(numBytes);
            return string.Format("{0} bytes ({1})", numBytes, human);
        }
    }
}
