// Copyright 2012, 2013, 2014 Andrew C. Dvorak
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.linkLabelSourceCode = new DotNetUtils.Controls.HyperlinkLabel();
            this.labelCopyright = new DotNetUtils.Controls.SelectableLabel();
            this.labelProductName = new DotNetUtils.Controls.SelectableLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.creditPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.textBoxLicense = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.textBoxSystemInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSystemInfo.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxSystemInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxSystemInfo.Font = new System.Drawing.Font("Courier New", 8F);
            this.textBoxSystemInfo.HideSelection = false;
            this.textBoxSystemInfo.Location = new System.Drawing.Point(6, 6);
            this.textBoxSystemInfo.Multiline = true;
            this.textBoxSystemInfo.Name = "textBoxSystemInfo";
            this.textBoxSystemInfo.ReadOnly = true;
            this.textBoxSystemInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSystemInfo.Size = new System.Drawing.Size(547, 361);
            this.textBoxSystemInfo.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(155, 71);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(567, 399);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBoxLicense);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(559, 373);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "License";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBoxSystemInfo);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(559, 373);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "System Info";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // linkLabelSourceCode
            // 
            this.linkLabelSourceCode.Cursor = System.Windows.Forms.Cursors.Default;
            this.linkLabelSourceCode.DisabledColor = System.Drawing.Color.Empty;
            this.linkLabelSourceCode.Enabled = false;
            this.linkLabelSourceCode.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelSourceCode.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelSourceCode.Location = new System.Drawing.Point(152, 51);
            this.linkLabelSourceCode.Name = "linkLabelSourceCode";
            this.linkLabelSourceCode.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelSourceCode.Size = new System.Drawing.Size(77, 14);
            this.linkLabelSourceCode.TabIndex = 5;
            this.linkLabelSourceCode.Text = "GitHub Project";
            // 
            // labelCopyright
            // 
            this.labelCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCopyright.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labelCopyright.Location = new System.Drawing.Point(155, 31);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.ReadOnly = true;
            this.labelCopyright.Size = new System.Drawing.Size(567, 13);
            this.labelCopyright.TabIndex = 4;
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.creditPanel);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(559, 373);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Credit";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // creditPanel
            // 
            this.creditPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.creditPanel.AutoScroll = true;
            this.creditPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.creditPanel.Location = new System.Drawing.Point(6, 6);
            this.creditPanel.Name = "creditPanel";
            this.creditPanel.Size = new System.Drawing.Size(547, 361);
            this.creditPanel.TabIndex = 2;
            this.creditPanel.WrapContents = false;
            // 
            // textBoxLicense
            // 
            this.textBoxLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLicense.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxLicense.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxLicense.Font = new System.Drawing.Font("Courier New", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLicense.HideSelection = false;
            this.textBoxLicense.Location = new System.Drawing.Point(6, 6);
            this.textBoxLicense.Multiline = true;
            this.textBoxLicense.Name = "textBoxLicense";
            this.textBoxLicense.ReadOnly = true;
            this.textBoxLicense.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLicense.Size = new System.Drawing.Size(547, 361);
            this.textBoxLicense.TabIndex = 9;
            // 
            // AboutBox
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOk;
            this.ClientSize = new System.Drawing.Size(734, 511);
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
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private DotNetUtils.Controls.SelectableLabel labelProductName;
        private DotNetUtils.Controls.SelectableLabel labelCopyright;
        private System.Windows.Forms.Button buttonOk;
        private DotNetUtils.Controls.HyperlinkLabel linkLabelSourceCode;
        private System.Windows.Forms.TextBox textBoxSystemInfo;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBoxLicense;
        private System.Windows.Forms.FlowLayoutPanel creditPanel;

    }
}
