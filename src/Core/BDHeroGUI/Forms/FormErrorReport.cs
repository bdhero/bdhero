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
        }

#if UseWPF

        /// <summary>
        ///     Windows Presentation Foundation editor
        /// </summary>
        private void InitEditor()
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
            Controls.Add(elementHost);
        }

#else

        /// <summary>
        ///     Windows Forms editor
        /// </summary>
        private void InitEditor()
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
            Controls.Add(editor);
        }

#endif
    }
}
