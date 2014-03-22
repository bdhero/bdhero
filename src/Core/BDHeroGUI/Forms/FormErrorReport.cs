using System;
using System.Windows.Forms;
using BDHero.ErrorReporting;
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

        private readonly ErrorReport _report;

        public FormErrorReport(ErrorReport report)
        {
            InitializeComponent();

            _report = report;

            InitMultilineEditor();
        }

        private void InitMultilineEditor()
        {
            var control = new TextEditorControl();
            var editor = control.Editor;

            if (editor.Options.SupportsWordWrap)
            {
                editor.Options.WordWrapIndent = editor.FontSize * 4;
            }
            else
            {
                checkBoxWordWrap.Hide();
            }

            editor.Load(FilePath);
            editor.SetSyntax(StandardSyntaxType.Markdown);

            textBoxTitle.Text = _report.Title;
            editor.Text = _report.Body;

            editor.TextChanged += EditorOnTextChanged;

            #region Options - checkbox events

            checkBoxShowLineNumbers.CheckedChanged +=
                (sender, args) => editor.Options.ShowLineNumbers = checkBoxShowLineNumbers.Checked;

            checkBoxShowRuler.CheckedChanged +=
                (sender, args) => editor.Options.ShowColumnRuler = checkBoxShowRuler.Checked;

            checkBoxShowWhitespace.CheckedChanged +=
                (sender, args) => editor.Options.ShowTabs = editor.Options.ShowSpaces = checkBoxShowWhitespace.Checked;

            checkBoxWordWrap.CheckedChanged +=
                (sender, args) => editor.Options.WordWrap = checkBoxWordWrap.Checked;

            #endregion

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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
