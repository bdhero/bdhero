using System;
using System.Windows.Forms;
using BDHero.ErrorReporting;
using OSUtils.Net;
using TextEditor;
using TextEditor.WinForms;

namespace BDHeroGUI.Forms
{
    public partial class FormErrorReport : Form
    {
        private readonly ErrorReport _report;
        private readonly INetworkStatusMonitor _networkStatusMonitor;

        private readonly TextEditorControl _editorControl;
        private readonly ITextEditor _editor;

        public FormErrorReport(ErrorReport report, INetworkStatusMonitor networkStatusMonitor)
        {
            InitializeComponent();

            _report = report;
            _networkStatusMonitor = networkStatusMonitor;

            _editorControl = new TextEditorControl();
            _editor = _editorControl.Editor;

            textBoxTitle.Text = _report.Title;
            _editor.Text = _report.Body;

            _editor.TextChanged += EditorOnTextChanged;
            _editor.SetSyntax(StandardSyntaxType.Markdown);

            #region Editor options

            _editor.Options.ShowWhiteSpace = false;

            if (_editor.Options.SupportsWordWrap)
                _editor.Options.WordWrapIndent = _editor.FontSize * 4;
            else
                checkBoxWordWrap.Hide();

            #endregion

            #region Options - checkbox events

            checkBoxShowLineNumbers.CheckedChanged +=
                (sender, args) => _editor.Options.ShowLineNumbers = checkBoxShowLineNumbers.Checked;

            checkBoxShowRuler.CheckedChanged +=
                (sender, args) => _editor.Options.ShowColumnRuler = checkBoxShowRuler.Checked;

            checkBoxShowWhitespace.CheckedChanged +=
                (sender, args) => _editor.Options.ShowWhiteSpace = checkBoxShowWhitespace.Checked;

            checkBoxWordWrap.CheckedChanged +=
                (sender, args) => _editor.Options.WordWrap = checkBoxWordWrap.Checked;

            #endregion

            _editorControl.Dock = DockStyle.Fill;
            panel1.Controls.Add(_editorControl);
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

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            if (!_networkStatusMonitor.IsOnline)
            {
                MessageBox.Show(this,
                                "You do not appear to be connected to the Internet." + "\n" +
                                "Please reconnect and try again.",
                                "Internet connection required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _report.Title = textBoxTitle.Text;
            _report.Body = _editor.Text;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
