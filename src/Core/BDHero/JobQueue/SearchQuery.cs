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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using I18N;

namespace BDHero.JobQueue
{
    public class SearchQuery
    {
        public string Title;
        public int? Year;
        public Language Language = Language.English;

        protected bool Equals(SearchQuery other)
        {
            return string.Equals(Title, other.Title) && Year == other.Year && Equals(Language, other.Language);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((SearchQuery) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Year.GetHashCode();
                hashCode = (hashCode * 397) ^ (Language != null ? Language.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            var iso639_2 = Language != null ? string.Format("{0}: ", Language.ISO_639_2) : "";
            return Year.HasValue
                       ? string.Format("{0}{1} ({2})", iso639_2, Title, Year)
                       : string.Format("{0}{1}", iso639_2, Title);
        }

        public void CopyFrom(SearchQuery other)
        {
            Title = other.Title;
            Year = other.Year;
            Language = other.Language;
        }
    }
}
