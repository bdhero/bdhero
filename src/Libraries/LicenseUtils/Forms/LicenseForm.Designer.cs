namespace LicenseUtils.Forms
{
    partial class LicenseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelName = new DotNetUtils.Controls.SelectableLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.hyperlinkLabelUrl = new DotNetUtils.Controls.HyperlinkLabel();
            this.hyperlinkLabelTlDrUrl = new DotNetUtils.Controls.HyperlinkLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.textBoxPlainText = new System.Windows.Forms.RichTextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.Location = new System.Drawing.Point(12, 12);
            this.labelName.Name = "labelName";
            this.labelName.ReadOnly = true;
            this.labelName.Size = new System.Drawing.Size(655, 16);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "License Name";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.hyperlinkLabelUrl);
            this.flowLayoutPanel1.Controls.Add(this.hyperlinkLabelTlDrUrl);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 35);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(655, 21);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // hyperlinkLabelUrl
            // 
            this.hyperlinkLabelUrl.Cursor = System.Windows.Forms.Cursors.Default;
            this.hyperlinkLabelUrl.DisabledColor = System.Drawing.Color.Empty;
            this.hyperlinkLabelUrl.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.hyperlinkLabelUrl.HoverColor = System.Drawing.Color.Empty;
            this.hyperlinkLabelUrl.Location = new System.Drawing.Point(0, 3);
            this.hyperlinkLabelUrl.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.hyperlinkLabelUrl.Name = "hyperlinkLabelUrl";
            this.hyperlinkLabelUrl.RegularColor = System.Drawing.Color.Empty;
            this.hyperlinkLabelUrl.Size = new System.Drawing.Size(47, 14);
            this.hyperlinkLabelUrl.TabIndex = 0;
            this.hyperlinkLabelUrl.Text = "Website";
            // 
            // hyperlinkLabelTlDrUrl
            // 
            this.hyperlinkLabelTlDrUrl.Cursor = System.Windows.Forms.Cursors.Default;
            this.hyperlinkLabelTlDrUrl.DisabledColor = System.Drawing.Color.Empty;
            this.hyperlinkLabelTlDrUrl.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.hyperlinkLabelTlDrUrl.HoverColor = System.Drawing.Color.Empty;
            this.hyperlinkLabelTlDrUrl.Location = new System.Drawing.Point(47, 3);
            this.hyperlinkLabelTlDrUrl.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.hyperlinkLabelTlDrUrl.Name = "hyperlinkLabelTlDrUrl";
            this.hyperlinkLabelTlDrUrl.RegularColor = System.Drawing.Color.Empty;
            this.hyperlinkLabelTlDrUrl.Size = new System.Drawing.Size(40, 14);
            this.hyperlinkLabelTlDrUrl.TabIndex = 1;
            this.hyperlinkLabelTlDrUrl.Text = "TL;DR";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 63);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(654, 347);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.webBrowser);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(646, 321);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Formatted";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBoxPlainText);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(646, 321);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Plain Text";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(592, 416);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "&OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPrint.Enabled = false;
            this.buttonPrint.Image = global::LicenseUtils.Properties.Resources.printer;
            this.buttonPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonPrint.Location = new System.Drawing.Point(12, 416);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(75, 23);
            this.buttonPrint.TabIndex = 4;
            this.buttonPrint.Text = "&Print";
            this.buttonPrint.UseVisualStyleBackColor = true;
            // 
            // webBrowser
            // 
            this.webBrowser.AllowNavigation = false;
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(3, 3);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(640, 315);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // textBoxPlainText
            // 
            this.textBoxPlainText.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxPlainText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPlainText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPlainText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlainText.HideSelection = false;
            this.textBoxPlainText.Location = new System.Drawing.Point(3, 3);
            this.textBoxPlainText.Name = "textBoxPlainText";
            this.textBoxPlainText.ReadOnly = true;
            this.textBoxPlainText.Size = new System.Drawing.Size(640, 315);
            this.textBoxPlainText.TabIndex = 0;
            this.textBoxPlainText.Text = "";
            this.textBoxPlainText.WordWrap = false;
            // 
            // LicenseForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOk;
            this.ClientSize = new System.Drawing.Size(679, 451);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.labelName);
            this.MinimizeBox = false;
            this.Name = "LicenseForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LicenseForm";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DotNetUtils.Controls.SelectableLabel labelName;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DotNetUtils.Controls.HyperlinkLabel hyperlinkLabelUrl;
        private DotNetUtils.Controls.HyperlinkLabel hyperlinkLabelTlDrUrl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.RichTextBox textBoxPlainText;

    }
}