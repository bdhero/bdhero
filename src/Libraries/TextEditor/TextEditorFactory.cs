using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextEditor
{
    public class TextEditorFactory
    {
        public static ITextEditor CreateTextEditor()
        {
#if __MonoCS__
            return new WinForms.TextEditorImpl();
#else
//            return new WPF.TextEditorImpl();
            return new WinForms.TextEditorImpl();
#endif
        }
    }
}
