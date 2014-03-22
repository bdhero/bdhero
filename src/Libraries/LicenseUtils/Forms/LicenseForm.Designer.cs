using UILib.WinForms.Controls;

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
            this.labelLicenseName = new SelectableLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.hyperlinkLabelUrl = new HyperlinkLabel();
            this.hyperlinkLabelTlDrUrl = new HyperlinkLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageFormatted = new System.Windows.Forms.TabPage();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.tabPagePlainText = new System.Windows.Forms.TabPage();
            this.textBoxPlainText = new System.Windows.Forms.RichTextBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelWorkName = new SelectableLabel();
            this.buttonPrint = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageFormatted.SuspendLayout();
            this.tabPagePlainText.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelLicenseName
            // 
            this.labelLicenseName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLicenseName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labelLicenseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLicenseName.Location = new System.Drawing.Point(12, 35);
            this.labelLicenseName.Name = "labelLicenseName";
            this.labelLicenseName.ReadOnly = true;
            this.labelLicenseName.Size = new System.Drawing.Size(655, 16);
            this.labelLicenseName.TabIndex = 2;
            this.labelLicenseName.Text = "License Name";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.hyperlinkLabelUrl);
            this.flowLayoutPanel1.Controls.Add(this.hyperlinkLabelTlDrUrl);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 57);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(655, 21);
            this.flowLayoutPanel1.TabIndex = 3;
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
            this.tabControl1.Controls.Add(this.tabPageFormatted);
            this.tabControl1.Controls.Add(this.tabPagePlainText);
            this.tabControl1.Location = new System.Drawing.Point(13, 84);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(654, 326);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPageFormatted
            // 
            this.tabPageFormatted.Controls.Add(this.webBrowser);
            this.tabPageFormatted.Location = new System.Drawing.Point(4, 22);
            this.tabPageFormatted.Name = "tabPageFormatted";
            this.tabPageFormatted.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFormatted.Size = new System.Drawing.Size(646, 300);
            this.tabPageFormatted.TabIndex = 0;
            this.tabPageFormatted.Text = "Formatted";
            this.tabPageFormatted.UseVisualStyleBackColor = true;
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
            this.webBrowser.Size = new System.Drawing.Size(640, 294);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // tabPagePlainText
            // 
            this.tabPagePlainText.Controls.Add(this.textBoxPlainText);
            this.tabPagePlainText.Location = new System.Drawing.Point(4, 22);
            this.tabPagePlainText.Name = "tabPagePlainText";
            this.tabPagePlainText.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePlainText.Size = new System.Drawing.Size(646, 300);
            this.tabPagePlainText.TabIndex = 1;
            this.tabPagePlainText.Text = "Plain Text";
            this.tabPagePlainText.UseVisualStyleBackColor = true;
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
            this.textBoxPlainText.Size = new System.Drawing.Size(640, 294);
            this.textBoxPlainText.TabIndex = 0;
            this.textBoxPlainText.Text = "";
            this.textBoxPlainText.WordWrap = false;
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
            // labelWorkName
            // 
            this.labelWorkName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelWorkName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labelWorkName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWorkName.Location = new System.Drawing.Point(12, 12);
            this.labelWorkName.Name = "labelWorkName";
            this.labelWorkName.ReadOnly = true;
            this.labelWorkName.Size = new System.Drawing.Size(655, 16);
            this.labelWorkName.TabIndex = 1;
            this.labelWorkName.Text = "Work Name";
            // 
            // buttonPrint
            // 
            this.buttonPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonPrint.Location = new System.Drawing.Point(12, 416);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(75, 23);
            this.buttonPrint.TabIndex = 5;
            this.buttonPrint.Text = "&Print";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
            // 
            // LicenseForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOk;
            this.ClientSize = new System.Drawing.Size(679, 451);
            this.Controls.Add(this.labelWorkName);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.labelLicenseName);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "LicenseForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LicenseForm";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageFormatted.ResumeLayout(false);
            this.tabPagePlainText.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SelectableLabel labelLicenseName;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private HyperlinkLabel hyperlinkLabelUrl;
        private HyperlinkLabel hyperlinkLabelTlDrUrl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageFormatted;
        private System.Windows.Forms.TabPage tabPagePlainText;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonPrint;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.RichTextBox textBoxPlainText;
        private SelectableLabel labelWorkName;

    }
}