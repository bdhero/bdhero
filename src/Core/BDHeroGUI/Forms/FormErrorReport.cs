using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Highlighting;

namespace BDHeroGUI.Forms
{
    public partial class FormErrorReport : Form
    {
        private const string FilePath = @"C:\projects\TestProject\CodeEditor\sample.md";
        private static readonly string FileExtension = Path.GetExtension(FilePath);

        public FormErrorReport()
        {
            InitializeComponent();

            InitEditor();
        }

#if __MonoCS__

        /// <summary>
        ///     Windows Forms editor
        /// </summary>
        private void InitEditor()
        {
            var wfEditor = new ICSharpCode.TextEditor.TextEditorControl();
            wfEditor.Dock = DockStyle.Fill;
            wfEditor.LoadFile(FilePath, true, true);
            splitContainer1.Panel2.Controls.Add(wfEditor);
        }

#else

        /// <summary>
        ///     Windows Presentation Foundation editor
        /// </summary>
        private void InitEditor()
        {
            var wpfEditor = new ICSharpCode.AvalonEdit.TextEditor();
            wpfEditor.Load(FilePath);

            // Font
            wpfEditor.FontFamily = new FontFamily("Consolas, Courier New, monospace");
            wpfEditor.FontSize = 14;

            #region Options

            // Options
            wpfEditor.ShowLineNumbers = true;
            wpfEditor.Options.CutCopyWholeLine = true;
            wpfEditor.Options.ShowColumnRuler = true;
            wpfEditor.Options.ColumnRulerPosition = 80;
            wpfEditor.Options.EnableRectangularSelection = true;
            wpfEditor.Options.EnableTextDragDrop = true;
            wpfEditor.Options.IndentationSize = 4;
            wpfEditor.Options.ShowBoxForControlCharacters = true;
            wpfEditor.Options.ShowSpaces = true;
            wpfEditor.Options.ShowTabs = true;

            #endregion

            var highlightingManager = HighlightingManager.Instance;
            wpfEditor.SyntaxHighlighting = highlightingManager.GetDefinitionByExtension(FileExtension);

            var elementHost = new ElementHost
                              {
                                  Child = wpfEditor,
                                  Dock = DockStyle.Fill
                              };
            this.Controls.Add(elementHost);
        }

#endif
    }
}
