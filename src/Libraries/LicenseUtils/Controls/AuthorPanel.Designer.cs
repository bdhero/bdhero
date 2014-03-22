using UILib.WinForms.Controls;

namespace LicenseUtils.Controls
{
    partial class AuthorPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelNotice = new SelectableLabel();
            this.emailLabel = new EmailLabel();
            this.hyperlinkLabel = new HyperlinkLabel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.labelNotice);
            this.flowLayoutPanel1.Controls.Add(this.emailLabel);
            this.flowLayoutPanel1.Controls.Add(this.hyperlinkLabel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(431, 21);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // labelNotice
            // 
            this.labelNotice.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labelNotice.Location = new System.Drawing.Point(0, 3);
            this.labelNotice.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.labelNotice.Name = "labelNotice";
            this.labelNotice.ReadOnly = true;
            this.labelNotice.Size = new System.Drawing.Size(139, 13);
            this.labelNotice.TabIndex = 0;
            this.labelNotice.Text = "© 2001-2005 Author Name";
            // 
            // emailLabel
            // 
            this.emailLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.emailLabel.DisabledColor = System.Drawing.Color.Empty;
            this.emailLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.emailLabel.HoverColor = System.Drawing.Color.Empty;
            this.emailLabel.Location = new System.Drawing.Point(139, 3);
            this.emailLabel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.RegularColor = System.Drawing.Color.Empty;
            this.emailLabel.Size = new System.Drawing.Size(74, 14);
            this.emailLabel.TabIndex = 1;
            this.emailLabel.Text = "Email Address";
            // 
            // hyperlinkLabel
            // 
            this.hyperlinkLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.hyperlinkLabel.DisabledColor = System.Drawing.Color.Empty;
            this.hyperlinkLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.hyperlinkLabel.HoverColor = System.Drawing.Color.Empty;
            this.hyperlinkLabel.Location = new System.Drawing.Point(213, 3);
            this.hyperlinkLabel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.hyperlinkLabel.Name = "hyperlinkLabel";
            this.hyperlinkLabel.RegularColor = System.Drawing.Color.Empty;
            this.hyperlinkLabel.Size = new System.Drawing.Size(60, 14);
            this.hyperlinkLabel.TabIndex = 2;
            this.hyperlinkLabel.Text = "Homepage";
            // 
            // AuthorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "AuthorPanel";
            this.Size = new System.Drawing.Size(431, 21);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private EmailLabel emailLabel;
        private HyperlinkLabel hyperlinkLabel;
        private SelectableLabel labelNotice;
    }
}
