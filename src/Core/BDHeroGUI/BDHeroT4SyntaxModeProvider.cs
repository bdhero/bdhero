using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextEditor.Resources;

namespace BDHeroGUI
{
    internal class BDHeroT4SyntaxModeProvider : T4SyntaxModeProvider
    {
        private static readonly Dictionary<string, string> Models
            = new Dictionary<string, string>
              {
                  { "BDHeroMovieFilePath.tt", new BDHeroMovieFilePathModeXshd().TransformText() },
              };

        public BDHeroT4SyntaxModeProvider()
            : base(Models)
        {
        }
    }
}
