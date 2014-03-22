using TextEditor.Resources.Syntax.Providers;

namespace TextEditor
{
    public class TextEditorFactory
    {
        private static ITextEditor CreateTextEditor()
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

        public static ITextEditor CreateMultiLineTextEditor()
        {
            var editor = CreateTextEditor();

            editor.Multiline = true;

            editor.Options.CutCopyWholeLine = true;
            editor.Options.ShowLineNumbers = true;
            editor.Options.ShowTabs = true;
            editor.Options.ShowSpaces = true;
            editor.Options.ShowColumnRuler = true;
            editor.Options.ColumnRulerPosition = 80;
            editor.Options.ConvertTabsToSpaces = true;
            editor.Options.IndentationSize = 4;

            return editor;
        }

        public static ITextEditor CreateSingleLineTextEditor()
        {
            var editor = CreateTextEditor();

            editor.Multiline = false;

            return editor;
        }
    }
}
