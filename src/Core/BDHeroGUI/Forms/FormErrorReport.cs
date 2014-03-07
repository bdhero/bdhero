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

            textBox1.Name = "TextBox";
        }

        private void InitMultilineEditor()
        {
            var control = new TextEditorControl();
            var editor = control.Editor;

            editor.Load(FilePath);
            editor.SetSyntax(StandardSyntaxType.Markdown);

            editor.TextChanged += EditorOnTextChanged;

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

            checkBoxDisabled.CheckedChanged +=
                (sender, args) => control.Enabled = !checkBoxDisabled.Checked;

            #endregion

            control.Name = "MultilineEditor";
            control.Dock = DockStyle.Fill;
            panel1.Controls.Add(control);
        }

        private void EditorOnTextChanged(object sender, EventArgs eventArgs)
        {
            var editor = sender as ITextEditor;
            if (editor == null)
                return;

            Text = Text.Replace("*", "");

            if (editor.IsModified)
                Text += "*";
        }

        private void InitSinglelineEditor()
        {
            textEditorControl1.Name = "SinglelineEditor";
            var editor = textEditorControl1.Editor;
            editor.LoadSyntaxDefinitions(new BDHeroT4SyntaxModeProvider());
            editor.SetSyntaxFromExtension(".bdheromoviefilepath");
            editor.Text += " --- " + new string(Path.GetInvalidPathChars()) + " --- " + new string(Path.GetInvalidFileNameChars());
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            buttonAccept.Text = string.Format("Accept: {0}", ++_numClicks);
        }
    }
}
