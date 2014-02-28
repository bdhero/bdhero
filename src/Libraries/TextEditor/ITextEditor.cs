using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Control = System.Windows.Forms.Control;

namespace TextEditor
{
    public interface ITextEditor
    {
        #region Critical

        ITextEditorOptions Options { get; }

        Control Control { get; }

        event EventHandler TextChanged;

        void SetSyntaxFromExtension(string fileNameOrExtension);

        string Text { get; set; }

        bool IsReadOnly { get; set; }

        void Load(Stream stream);

        void Load(string filePath);

        void Save(Stream stream);

        void Save(string filePath);

        void SelectAll();

        string SelectedText { get; set; }

        int SelectionLength { get; }

        void Cut();

        void Copy();

        void Paste();

        void Print();

        #endregion

        #region Nice to have

        bool IsModified { get; }

        int LineCount { get; }

        #endregion
    }
}
