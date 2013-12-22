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
