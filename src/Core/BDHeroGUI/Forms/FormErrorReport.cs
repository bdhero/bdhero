using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using TextEditor;
using TextEditor.WinForms;

namespace BDHeroGUI.Forms
{
    public partial class FormErrorReport : Form
    {
#if __MonoCS__
        private const string FilePath = @"/Users/admin/Documents/sample.md";
#else
        private const string FilePath = @"C:\projects\TestProject\CodeEditor\sample.md";
#endif

        private int _numClicks;

        public FormErrorReport()
        {
            InitializeComponent();

            InitMultilineEditor();
            InitSinglelineEditor();
        }

        private void InitMultilineEditor()
        {
            var control = new TextEditorControl();
            var editor = control.Editor;

            editor.Options.ShowLineNumbers = true;
            editor.Options.ShowTabs = true;
            editor.Options.ShowSpaces = true;
            editor.Options.ShowColumnRuler = true;
            editor.Options.ColumnRulerPosition = 80;
            editor.Options.ConvertTabsToSpaces = true;
            editor.Options.IndentationSize = 4;
            editor.FontSize = 14;

            editor.SetSyntax(StandardSyntaxType.Markdown);
            editor.Load(FilePath);

            #region Options - checkbox events

            checkBoxMultiline.CheckedChanged +=
                (sender, args) => editor.Multiline = checkBoxMultiline.Checked;

            checkBoxShowLineNumbers.CheckedChanged +=
                (sender, args) => editor.Options.ShowLineNumbers = checkBoxShowLineNumbers.Checked;

            checkBoxShowRuler.CheckedChanged +=
                (sender, args) => editor.Options.ShowColumnRuler = checkBoxShowRuler.Checked;

            checkBoxShowWhitespace.CheckedChanged +=
                (sender, args) => editor.Options.ShowTabs = editor.Options.ShowSpaces = checkBoxShowWhitespace.Checked;

            checkBoxReadonly.CheckedChanged +=
                (sender, args) => editor.ReadOnly = checkBoxReadonly.Checked;

            #endregion

            control.Dock = DockStyle.Fill;
            panel1.Controls.Add(control);
        }

        private void InitSinglelineEditor()
        {
            var editor = textEditorControl1.Editor;
            editor.SetSyntax(StandardSyntaxType.FilePath);
            editor.Text += " --- " + new string(Path.GetInvalidPathChars()) + " --- " + new string(Path.GetInvalidFileNameChars());
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            buttonAccept.Text = string.Format("Accept: {0}", ++_numClicks);
        }
    }
}
