using System.Collections.Generic;
using TextEditor.Resources.Syntax.Providers;

namespace BDHeroGUI
{
    internal class BDHeroT4SyntaxModeProvider : T4SyntaxModeProvider
    {
        private static readonly Dictionary<string, string> Models
            = new Dictionary<string, string>
              {
                  { "BDHeroMovieFileName.tt", new BDHeroMovieFileNameModeXshd().TransformText().Trim() },
                  { "BDHeroTVShowFileName.tt", new BDHeroTVShowFileNameModeXshd().TransformText().Trim() },
                  { "BDHeroMovieFilePath.tt", new BDHeroMovieFilePathModeXshd().TransformText().Trim() },
                  { "BDHeroTVShowFilePath.tt", new BDHeroTVShowFilePathModeXshd().TransformText().Trim() },
              };

        public BDHeroT4SyntaxModeProvider()
            : base(Models)
        {
        }
    }
}
