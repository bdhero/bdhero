using System.Collections.Generic;

namespace BDHero
{
    public static class FileNameVars
    {
        public static readonly List<string> MovieVars
            = new List<string>
              {
                  "volume",
                  "title",
                  "year",
                  "res",
                  "vcodec",
                  "acodec",
                  "channels",
                  "cut",
                  "vlang",
                  "alang",
              };

        public static readonly List<string> TVShowVars
            = new List<string>
              {
                  "volume",
                  "title",
                  "date",
                  "res",
                  "vcodec",
                  "acodec",
                  "channels",
                  "cut",
                  "vlang",
                  "alang",
                  "episodetitle",
                  "season",
                  "episode",
              };
    }
}