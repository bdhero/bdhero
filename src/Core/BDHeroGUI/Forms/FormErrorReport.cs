#if !__MonoCS__
#define UseWPF
#endif

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNetUtils;
using TextEditor;
#if UseWPF
using System.Windows.Media;
using System.Windows.Forms.Integration;
using ICSharpCode.AvalonEdit.Highlighting;
#else
using ICSharpCode.TextEditor.Document;
#endif

namespace BDHeroGUI.Forms
{
    public partial class FormErrorReport : Form
    {
#if __MonoCS__
        private const string FilePath = @"/Users/admin/Documents/sample.md";
#else
        private const string FilePath = @"C:\projects\TestProject\CodeEditor\sample.md";
#endif
        private static readonly string FileExtension = Path.GetExtension(FilePath);

        public FormErrorReport()
        {
            InitializeComponent();

            InitEditor();
//            InitEditorInline();
        }

        private void InitEditor()
        {
            var editor = TextEditorFactory.CreateTextEditor();

            editor.Options.ShowLineNumbers = true;
            editor.Options.ShowTabs = true;
            editor.Options.ShowSpaces = true;
            editor.Options.ShowColumnRuler = true;
            editor.Options.ColumnRulerPosition = 80;
            editor.Options.ConvertTabsToSpaces = true;
            editor.Options.IndentationSize = 4;
            editor.Options.FontSize = 14;

            editor.SetSyntaxFromExtension(FilePath);
            editor.Load(FilePath);

            editor.TextChanged += (sender, args) => LogEvent(editor, "TextChanged");

            #region Options - checkbox events

            checkBoxMultiline.CheckedChanged +=
                (sender, args) => editor.Options.Multiline = checkBoxMultiline.Checked;

            checkBoxShowLineNumbers.CheckedChanged +=
                (sender, args) => editor.Options.ShowLineNumbers = checkBoxShowLineNumbers.Checked;

            checkBoxShowRuler.CheckedChanged +=
                (sender, args) => editor.Options.ShowColumnRuler = checkBoxShowRuler.Checked;

            checkBoxShowWhitespace.CheckedChanged +=
                (sender, args) => editor.Options.ShowTabs = editor.Options.ShowSpaces = checkBoxShowWhitespace.Checked;

            checkBoxReadonly.CheckedChanged +=
                (sender, args) => editor.IsReadOnly = checkBoxReadonly.Checked;

            #endregion

            var control = editor.Control;
            control.Dock = DockStyle.Fill;
            panel1.Controls.Add(control);
        }

        private void LogEvent(ITextEditor editor, string eventName)
        {
            Console.WriteLine("{0}: {{ modified = {1}, readonly = {2}, linecount = {3}, selectedtext = \"{4}\", selectionlength = {5} }}",
                eventName,
                editor.IsModified,
                editor.IsReadOnly,
                editor.LineCount,
                editor.SelectedText,
                editor.SelectionLength);
        }

#if UseWPF

        /// <summary>
        ///     Windows Presentation Foundation editor
        /// </summary>
        private void InitEditorInline()
        {
            var editor = new ICSharpCode.AvalonEdit.TextEditor();
            editor.Load(FilePath);

            // Font
            editor.FontFamily = new FontFamily("Consolas, Courier New, monospace");
            editor.FontSize = 14;

            #region Options

            // Line numbers
            editor.ShowLineNumbers = true;

            // Column ruler
            editor.Options.ShowColumnRuler = true;
            editor.Options.ColumnRulerPosition = 80;

            // Selection and drag/drop
            editor.Options.CutCopyWholeLine = true;
            editor.Options.EnableRectangularSelection = true;
            editor.Options.EnableTextDragDrop = true;
            
            // Indentation
            editor.Options.IndentationSize = 4;
            editor.Options.ConvertTabsToSpaces = true;
            
            // Character visualization
            editor.Options.ShowBoxForControlCharacters = true;
            editor.Options.ShowSpaces = true;
            editor.Options.ShowTabs = true;

            #endregion

            var highlightingManager = HighlightingManager.Instance;
            editor.SyntaxHighlighting = highlightingManager.GetDefinitionByExtension(FileExtension);

            var elementHost = new ElementHost
                              {
                                  Child = editor,
                                  Dock = DockStyle.Fill
                              };
            panel1.Controls.Add(elementHost);
        }

#else

        /// <summary>
        ///     Windows Forms editor
        /// </summary>
        private void InitEditorInline()
        {
            var editor = new ICSharpCode.TextEditor.TextEditorControl();
            editor.LoadFile(FilePath, true, true);

            #region Options

            // Hotkeys
//            editor.

            // Line numbers
            editor.ShowLineNumbers = true;

            // Column ruler
            editor.ShowVRuler = true;
            editor.VRulerRow = 80;

            // Selection and drag/drop
//            editor.Options.CutCopyWholeLine = true;
//            editor.Options.EnableRectangularSelection = true;
//            editor.Options.EnableTextDragDrop = true;

            // Indentation
            editor.TabIndent = 4;
            editor.ConvertTabsToSpaces = true;
            editor.IndentStyle = IndentStyle.Smart;

            // Character visualization
//            editor.Options.ShowBoxForControlCharacters = true;
            editor.ShowSpaces = true;
            editor.ShowTabs = true;

            #endregion

            try
            {
                var dir = AssemblyUtils.GetInstallDir(GetType());
                if (Directory.Exists(dir))
                {
                    var fsmProvider = new FileSyntaxModeProvider(dir); // Provider
                    HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider);
                }

                editor.SetHighlighting("MarkDown");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }

            editor.Dock = DockStyle.Fill;
            panel1.Controls.Add(editor);
        }

#endif
    }
}
