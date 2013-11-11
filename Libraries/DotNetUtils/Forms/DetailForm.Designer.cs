namespace DotNetUtils.Forms
{
    sealed partial class DetailForm
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
            this.buttonOk = new System.Windows.Forms.Button();
            this.systemIcon = new System.Windows.Forms.PictureBox();
            this.panel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBoxDetails = new System.Windows.Forms.RichTextBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copySelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.selectallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBoxShowDetails = new System.Windows.Forms.CheckBox();
            this.labelSummary = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.systemIcon)).BeginInit();
            this.panel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOk.Location = new System.Drawing.Point(397, 226);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "&OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // systemIcon
            // 
            this.systemIcon.Location = new System.Drawing.Point(12, 12);
            this.systemIcon.Name = "systemIcon";
            this.systemIcon.Size = new System.Drawing.Size(32, 32);
            this.systemIcon.TabIndex = 1;
            this.systemIcon.TabStop = false;
            // 
            // panel
            // 
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.Controls.Add(this.panel2);
            this.panel.Controls.Add(this.checkBoxShowDetails);
            this.panel.Controls.Add(this.labelSummary);
            this.panel.Location = new System.Drawing.Point(50, 13);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(422, 207);
            this.panel.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBoxDetails);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(422, 177);
            this.panel2.TabIndex = 1;
            // 
            // textBoxDetails
            // 
            this.textBoxDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDetails.ContextMenuStrip = this.contextMenuStrip;
            this.textBoxDetails.HideSelection = false;
            this.textBoxDetails.Location = new System.Drawing.Point(3, 5);
            this.textBoxDetails.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.textBoxDetails.Name = "textBoxDetails";
            this.textBoxDetails.ReadOnly = true;
            this.textBoxDetails.Size = new System.Drawing.Size(416, 164);
            this.textBoxDetails.TabIndex = 1;
            this.textBoxDetails.Text = "";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copySelectedToolStripMenuItem,
            this.copyAllToolStripMenuItem,
            this.toolStripMenuItem1,
            this.selectallToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(149, 76);
            // 
            // copySelectedToolStripMenuItem
            // 
            this.copySelectedToolStripMenuItem.Name = "copySelectedToolStripMenuItem";
            this.copySelectedToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.copySelectedToolStripMenuItem.Text = "Copy &selected";
            this.copySelectedToolStripMenuItem.Click += new System.EventHandler(this.copySelectedToolStripMenuItem_Click);
            // 
            // copyAllToolStripMenuItem
            // 
            this.copyAllToolStripMenuItem.Name = "copyAllToolStripMenuItem";
            this.copyAllToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.copyAllToolStripMenuItem.Text = "&Copy all";
            this.copyAllToolStripMenuItem.Click += new System.EventHandler(this.copyAllToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(145, 6);
            // 
            // selectallToolStripMenuItem
            // 
            this.selectallToolStripMenuItem.Name = "selectallToolStripMenuItem";
            this.selectallToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.selectallToolStripMenuItem.Text = "Select &all";
            this.selectallToolStripMenuItem.Click += new System.EventHandler(this.selectallToolStripMenuItem_Click);
            // 
            // checkBoxShowDetails
            // 
            this.checkBoxShowDetails.AutoSize = true;
            this.checkBoxShowDetails.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkBoxShowDetails.Location = new System.Drawing.Point(0, 190);
            this.checkBoxShowDetails.Name = "checkBoxShowDetails";
            this.checkBoxShowDetails.Size = new System.Drawing.Size(422, 17);
            this.checkBoxShowDetails.TabIndex = 0;
            this.checkBoxShowDetails.Text = "Show &Details";
            this.checkBoxShowDetails.UseVisualStyleBackColor = true;
            // 
            // labelSummary
            // 
            this.labelSummary.AutoSize = true;
            this.labelSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSummary.Location = new System.Drawing.Point(0, 0);
            this.labelSummary.Name = "labelSummary";
            this.labelSummary.Size = new System.Drawing.Size(100, 13);
            this.labelSummary.TabIndex = 0;
            this.labelSummary.Text = "Exception Message";
            // 
            // DetailForm
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.buttonOk;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.systemIcon);
            this.Controls.Add(this.buttonOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DetailForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DetailForm";
            ((System.ComponentModel.ISupportInitialize)(this.systemIcon)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.PictureBox systemIcon;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.CheckBox checkBoxShowDetails;
        private System.Windows.Forms.RichTextBox textBoxDetails;
        private System.Windows.Forms.Label labelSummary;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copySelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem selectallToolStripMenuItem;
    }
}