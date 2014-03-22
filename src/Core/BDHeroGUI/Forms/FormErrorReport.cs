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

        private readonly ITextEditor _editor;

        public FormErrorReport(ErrorReport report, INetworkStatusMonitor networkStatusMonitor)
        {
            InitializeComponent();

            _report = report;
            _networkStatusMonitor = networkStatusMonitor;

            var editorControl = new TextEditorControl();
            _editor = editorControl.Editor;

            textBoxTitle.Text = _report.Title;
            _editor.Text = _report.Body;

            _editor.Options.ShowWhiteSpace = false;
            _editor.Options.ShowLineNumbers = false;

            _editor.TextChanged += EditorOnTextChanged;
            _editor.SetSyntax(StandardSyntaxType.Markdown);

            editorControl.Dock = DockStyle.Fill;
            editorPanel.Controls.Add(editorControl);
        }

        private void EditorOnTextChanged(object sender, EventArgs eventArgs)
        {
            Text = Text.Replace("*", "");

            if (_editor.IsModified)
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
