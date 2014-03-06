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
                  { "BDHeroMovieFileName.tt", new BDHeroMovieFileNameModeXshd().TransformText().Trim() },
              };

        public BDHeroT4SyntaxModeProvider()
            : base(Models)
        {
        }
    }
}
