﻿namespace BDHeroGUI.Components
{
    partial class MediaPanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.comboBoxMedia = new System.Windows.Forms.ComboBox();
            this.linkLabelSearch = new DotNetUtils.Controls.LinkLabel2();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Media:";
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(0, 16);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.pictureBox);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.comboBoxMedia);
            this.splitContainer.Panel2.Controls.Add(this.linkLabelSearch);
            this.splitContainer.Size = new System.Drawing.Size(360, 161);
            this.splitContainer.SplitterDistance = 119;
            this.splitContainer.TabIndex = 0;
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(119, 161);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            // 
            // comboBoxMedia
            // 
            this.comboBoxMedia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMedia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMedia.FormattingEnabled = true;
            this.comboBoxMedia.Location = new System.Drawing.Point(4, 4);
            this.comboBoxMedia.Name = "comboBoxMedia";
            this.comboBoxMedia.Size = new System.Drawing.Size(230, 21);
            this.comboBoxMedia.TabIndex = 0;
            // 
            // linkLabelSearch
            // 
            this.linkLabelSearch.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelSearch.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelSearch.Location = new System.Drawing.Point(2, 31);
            this.linkLabelSearch.Name = "linkLabelSearch";
            this.linkLabelSearch.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelSearch.Size = new System.Drawing.Size(51, 14);
            this.linkLabelSearch.TabIndex = 1;
            this.linkLabelSearch.Text = "Search...";
            this.linkLabelSearch.Click += new System.EventHandler(this.linkLabelSearch_Click);
            // 
            // MediaPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.label1);
            this.Name = "MediaPanel";
            this.Size = new System.Drawing.Size(360, 180);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.PictureBox pictureBox;
        private DotNetUtils.Controls.LinkLabel2 linkLabelSearch;
        private System.Windows.Forms.ComboBox comboBoxMedia;
    }
}
