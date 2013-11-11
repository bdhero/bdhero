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
