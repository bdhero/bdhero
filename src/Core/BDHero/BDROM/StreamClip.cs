// Copyright 2012, 2013, 2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

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
