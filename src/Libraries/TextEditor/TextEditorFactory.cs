using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextEditor.Resources;

namespace TextEditor
{
    public class TextEditorFactory
    {
        public static ITextEditor CreateTextEditor()
        {
            ITextEditor editor =
#if __MonoCS__
                new WinForms.TextEditorImpl()
#else
                new WPF.TextEditorImpl()
//                new WinForms.TextEditorImpl()
#endif
                ;

            // Default value
            editor.FontSize = 14;

            // Load default syntax highlighting mode definitions
            editor.LoadSyntaxDefinitions(new T4SyntaxModeProvider());
            
            return editor;
        }
    }
}
