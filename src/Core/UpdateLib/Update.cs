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

namespace UpdateLib
{
    public class Update
    {
        public readonly Version Version;
        public readonly string FileName;
        public readonly string Uri;
        public readonly string SHA1;
        public readonly long Size;

        public Update(Version version, string fileName, string uri, string sha1, long size)
        {
            Version = version;
            FileName = fileName;
            Uri = uri;
            SHA1 = sha1;
            Size = size;
        }
    }
}