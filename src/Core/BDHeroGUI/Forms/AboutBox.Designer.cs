// Copyright 2012-2014 Andrew C. Dvorak
//
// This file is part of BDHero.
//
// BDHero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BDHero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with BDHero.  If not, see <http://www.gnu.org/licenses/>.

namespace BDHeroGUI.Forms
{
    sealed partial class AboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.textBoxSystemInfo = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageCredit = new System.Windows.Forms.TabPage();
            this.creditPanel = new System.Windows.Forms.Panel();
            this.tabPageSysInfo = new System.Windows.Forms.TabPage();
            this.linkLabelSourceCode = new UILib.Controls.HyperlinkLabel();
            this.labelCopyright = new UILib.Controls.SelectableLabel();
            this.labelProductName = new UILib.Controls.SelectableLabel();
            this.labelBuildDate = new UILib.Controls.SelectableLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageCredit.SuspendLayout();
            this.tabPageSysInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Image = global::BDHeroGUI.Properties.Resources.bdhero_gui_128;
            this.pictureBoxLogo.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(128, 128);
            this.pictureBoxLogo.TabIndex = 0;
            this.pictureBoxLogo.TabStop = false;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOk.Location = new System.Drawing.Point(647, 476);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "&OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // textBoxSystemInfo
            // 
            this.textBoxSystemInfo.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxSystemInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxSystemInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSystemInfo.Font = new System.Drawing.Font("Courier New", 8F);
            this.textBoxSystemInfo.HideSelection = false;
            this.textBoxSystemInfo.Location = new System.Drawing.Point(3, 3);
            this.textBoxSystemInfo.Multiline = true;
            this.textBoxSystemInfo.Name = "textBoxSystemInfo";
            this.textBoxSystemInfo.ReadOnly = true;
            this.textBoxSystemInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSystemInfo.Size = new System.Drawing.Size(553, 347);
            this.textBoxSystemInfo.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageCredit);
            this.tabControl1.Controls.Add(this.tabPageSysInfo);
            this.tabControl1.Location = new System.Drawing.Point(155, 91);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(567, 379);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPageCredit
            // 
            this.tabPageCredit.Controls.Add(this.creditPanel);
            this.tabPageCredit.Location = new System.Drawing.Point(4, 22);
            this.tabPageCredit.Name = "tabPageCredit";
            this.tabPageCredit.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCredit.Size = new System.Drawing.Size(559, 353);
            this.tabPageCredit.TabIndex = 2;
            this.tabPageCredit.Text = "Credit";
            this.tabPageCredit.UseVisualStyleBackColor = true;
            // 
            // creditPanel
            // 
            this.creditPanel.AutoScroll = true;
            this.creditPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.creditPanel.Location = new System.Drawing.Point(3, 3);
            this.creditPanel.Name = "creditPanel";
            this.creditPanel.Size = new System.Drawing.Size(553, 347);
            this.creditPanel.TabIndex = 0;
            // 
            // tabPageSysInfo
            // 
            this.tabPageSysInfo.Controls.Add(this.textBoxSystemInfo);
            this.tabPageSysInfo.Location = new System.Drawing.Point(4, 22);
            this.tabPageSysInfo.Name = "tabPageSysInfo";
            this.tabPageSysInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSysInfo.Size = new System.Drawing.Size(559, 353);
            this.tabPageSysInfo.TabIndex = 1;
            this.tabPageSysInfo.Text = "System Info";
            this.tabPageSysInfo.UseVisualStyleBackColor = true;
            // 
            // linkLabelSourceCode
            // 
            this.linkLabelSourceCode.Cursor = System.Windows.Forms.Cursors.Default;
            this.linkLabelSourceCode.DisabledColor = System.Drawing.Color.Empty;
            this.linkLabelSourceCode.Enabled = false;
            this.linkLabelSourceCode.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelSourceCode.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelSourceCode.Location = new System.Drawing.Point(152, 71);
            this.linkLabelSourceCode.Name = "linkLabelSourceCode";
            this.linkLabelSourceCode.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelSourceCode.Size = new System.Drawing.Size(77, 14);
            this.linkLabelSourceCode.TabIndex = 4;
            this.linkLabelSourceCode.Text = "GitHub Project";
            // 
            // labelCopyright
            // 
            this.labelCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCopyright.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labelCopyright.Location = new System.Drawing.Point(155, 51);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.ReadOnly = true;
            this.labelCopyright.Size = new System.Drawing.Size(567, 13);
            this.labelCopyright.TabIndex = 3;
            this.labelCopyright.Text = "Copyright";
            // 
            // labelProductName
            // 
            this.labelProductName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProductName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labelProductName.Location = new System.Drawing.Point(155, 12);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.ReadOnly = true;
            this.labelProductName.Size = new System.Drawing.Size(567, 13);
            this.labelProductName.TabIndex = 1;
            this.labelProductName.Text = "Product Name";
            // 
            // labelBuildDate
            // 
            this.labelBuildDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBuildDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labelBuildDate.Location = new System.Drawing.Point(155, 32);
            this.labelBuildDate.Name = "labelBuildDate";
            this.labelBuildDate.ReadOnly = true;
            this.labelBuildDate.Size = new System.Drawing.Size(567, 13);
            this.labelBuildDate.TabIndex = 2;
            this.labelBuildDate.Text = "Built on";
            // 
            // AboutBox
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOk;
            this.ClientSize = new System.Drawing.Size(734, 511);
            this.Controls.Add(this.labelBuildDate);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.linkLabelSourceCode);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.labelCopyright);
            this.Controls.Add(this.labelProductName);
            this.Controls.Add(this.pictureBoxLogo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 250);
            this.Name = "AboutBox";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageCredit.ResumeLayout(false);
            this.tabPageSysInfo.ResumeLayout(false);
            this.tabPageSysInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private UILib.Controls.SelectableLabel labelProductName;
        private UILib.Controls.SelectableLabel labelCopyright;
        private System.Windows.Forms.Button buttonOk;
        private UILib.Controls.HyperlinkLabel linkLabelSourceCode;
        private System.Windows.Forms.TextBox textBoxSystemInfo;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSysInfo;
        private System.Windows.Forms.TabPage tabPageCredit;
        private System.Windows.Forms.Panel creditPanel;
        private UILib.Controls.SelectableLabel labelBuildDate;

    }
}
