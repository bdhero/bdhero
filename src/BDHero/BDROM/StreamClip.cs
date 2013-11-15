using System;
using System.IO;

namespace BDHero.BDROM
{
    /// <summary>
    /// Represents a .M2TS file on a BD-ROM.
    /// </summary>
    public class StreamClip
    {
        #region Readonly fields

        public readonly FileInfo FileInfo;
        public readonly String FileName;
        public readonly ulong FileSize;
        public readonly int Index;
        public readonly int AngleIndex;
        public readonly TimeSpan Length;

        #endregion

        #region Constructors

        public StreamClip(FileInfo fileInfo, string fileName, ulong fileSize, int index, int angleIndex, double lengthSec)
        {
            FileInfo = fileInfo;
            FileName = fileName;
            FileSize = fileSize;
            Index = index;
            AngleIndex = angleIndex;
            Length = TimeSpan.FromMilliseconds(lengthSec * 1000);
        }

        public StreamClip(FileInfo fileInfo, string fileName, ulong fileSize, int index, int angleIndex, TimeSpan length)
        {
            FileInfo = fileInfo;
            FileName = fileName;
            FileSize = fileSize;
            Index = index;
            AngleIndex = angleIndex;
            Length = length;
        }

        #endregion
    }
}
