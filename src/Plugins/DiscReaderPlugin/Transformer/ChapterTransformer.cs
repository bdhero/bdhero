using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.BDROM;

namespace BDHero.Plugin.DiscReader.Transformer
{
    static class ChapterTransformer
    {
        public static IList<Chapter> Transform(IEnumerable<double> chaptersInSeconds)
        {
            return chaptersInSeconds.Select((t, i) => new Chapter(i + 1, t)).ToList();
        }
    }
}
