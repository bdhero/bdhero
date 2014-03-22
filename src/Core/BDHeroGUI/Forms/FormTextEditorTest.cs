using System;
using System.IO;
using System.Windows.Forms;
using TextEditor;
using TextEditor.WinForms;

namespace BDHeroGUI.Forms
{
    public partial class FormTextEditorTest : Form
    {
#if __MonoCS__
        private const string FilePath = @"/Users/admin/Documents/sample.md";
#else
        private const string FilePath = @"C:\projects\TestProject\CodeEditor\sample.md";
#endif

        private int _numClicks;

        public FormTextEditorTest()
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
                (sender, args) => editor.Options.ShowWhiteSpace = checkBoxShowWhitespace.Checked;

            checkBoxReadonly.CheckedChanged +=
                (sender, args) => editor.ReadOnly = checkBoxReadonly.Checked;

            checkBoxDisabled.CheckedChanged +=
                (sender, args) => control.Enabled = !checkBoxDisabled.Checked;

            checkBoxBorder.CheckedChanged +=
                (sender, args) => control.BorderStyle = checkBoxBorder.Checked ? BorderStyle.Fixed3D : BorderStyle.None;

            numericUpDownPadding.ValueChanged +=
                (sender, args) => control.Padding = new Padding((int)numericUpDownPadding.Value);

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

            checkBoxBorder.CheckedChanged +=
                (sender, args) => textBox1.BorderStyle = checkBoxBorder.Checked ? BorderStyle.Fixed3D : BorderStyle.None;

            checkBoxBorder.CheckedChanged +=
                (sender, args) => textEditorControl1.BorderStyle = checkBoxBorder.Checked ? BorderStyle.Fixed3D : BorderStyle.None;
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            buttonAccept.Text = string.Format("Accept: {0}", ++_numClicks);
        }
    }
}
