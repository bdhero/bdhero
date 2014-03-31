using System;
using System.Windows.Forms;
using BDHero.ErrorReporting;
using DotNetUtils;
using OSUtils.Net;
using TextEditor;
using TextEditor.WinForms;
using UpdateLib;

namespace BDHeroGUI.Forms
{
    public partial class FormErrorReport : Form
    {
        private readonly ErrorReport _report;
        private readonly INetworkStatusMonitor _networkStatusMonitor;
        private readonly UpdateClient _updateClient;

        private readonly ITextEditor _editor;

        public FormErrorReport(ErrorReport report, INetworkStatusMonitor networkStatusMonitor, UpdateClient updateClient)
        {
            InitializeComponent();

            _report = report;
            _networkStatusMonitor = networkStatusMonitor;
            _updateClient = updateClient;

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
            if (_updateClient.IsUpdateAvailable)
            {
                var message = string.Format("{0} is out of date.  To report errors, you need the latest version." + "\n" +
                                            "You can download the latest and greatest from the Help menu.", AppUtils.ProductName);
                MessageBox.Show(this, message, "Update required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_networkStatusMonitor.IsOnline)
            {
                var message = string.Format("You do not appear to be connected to the Internet." + "\n" +
                                            "Please reconnect and try again.");
                MessageBox.Show(this, message, "Internet connection required", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
