using System.Collections.Generic;

namespace BDHero.BDROM
{
    public class ChapterSearchResult
    {
        public string Title;
        public IList<Chapter> Chapters;

        public bool IsSelected;

        public override string ToString()
        {
            return Title;
        }
    }
}