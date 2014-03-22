using System.Collections.Generic;
using TextEditor.Resources.Syntax.Providers;

namespace BDHero.SyntaxHighlighting
{
    public class BDHeroT4SyntaxModeProvider : T4SyntaxModeProvider
    {
        private static readonly Dictionary<string, string> Models
            = new Dictionary<string, string>
              {
                  { "MovieNameXshd.tt",  new MovieNameXshd().TransformText().Trim()  },
                  { "TVShowNameXshd.tt", new TVShowNameXshd().TransformText().Trim() },
                  { "MoviePathXshd.tt",  new MoviePathXshd().TransformText().Trim()  },
                  { "TVShowPathXshd.tt", new TVShowPathXshd().TransformText().Trim() },
              };

        public BDHeroT4SyntaxModeProvider()
            : base(Models)
        {
        }
    }
}
