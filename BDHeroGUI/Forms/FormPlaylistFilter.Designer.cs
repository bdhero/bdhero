namespace BDHeroGUI.Forms
{
    partial class FormPlaylistFilter
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkedListBoxTypes = new System.Windows.Forms.CheckedListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxMinChapterCount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxMinDuration = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxHideHiddenFirstTracks = new System.Windows.Forms.CheckBox();
            this.checkBoxHideLoops = new System.Windows.Forms.CheckBox();
            this.checkBoxHideDuplicateStreamClips = new System.Windows.Forms.CheckBox();
            this.checkBoxHideDuplicatePlaylists = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.checkedListBoxTypes);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxMinChapterCount);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxMinDuration);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(278, 175);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Show";
            // 
            // checkedListBoxTypes
            // 
            this.checkedListBoxTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxTypes.FormattingEnabled = true;
            this.checkedListBoxTypes.Location = new System.Drawing.Point(89, 70);
            this.checkedListBoxTypes.Name = "checkedListBoxTypes";
            this.checkedListBoxTypes.Size = new System.Drawing.Size(174, 94);
            this.checkedListBoxTypes.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Types:";
            // 
            // textBoxMinChapterCount
            // 
            this.textBoxMinChapterCount.Location = new System.Drawing.Point(89, 43);
            this.textBoxMinChapterCount.Name = "textBoxMinChapterCount";
            this.textBoxMinChapterCount.Size = new System.Drawing.Size(83, 20);
            this.textBoxMinChapterCount.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(64, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = ">=";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Chapters:";
            // 
            // textBoxMinDuration
            // 
            this.textBoxMinDuration.Location = new System.Drawing.Point(89, 17);
            this.textBoxMinDuration.Name = "textBoxMinDuration";
            this.textBoxMinDuration.Size = new System.Drawing.Size(83, 20);
            this.textBoxMinDuration.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = ">=";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Duration:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBoxHideHiddenFirstTracks);
            this.groupBox2.Controls.Add(this.checkBoxHideLoops);
            this.groupBox2.Controls.Add(this.checkBoxHideDuplicateStreamClips);
            this.groupBox2.Controls.Add(this.checkBoxHideDuplicatePlaylists);
            this.groupBox2.Location = new System.Drawing.Point(13, 195);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 115);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Hide";
            // 
            // checkBoxHideHiddenFirstTracks
            // 
            this.checkBoxHideHiddenFirstTracks.AutoSize = true;
            this.checkBoxHideHiddenFirstTracks.Location = new System.Drawing.Point(10, 91);
            this.checkBoxHideHiddenFirstTracks.Name = "checkBoxHideHiddenFirstTracks";
            this.checkBoxHideHiddenFirstTracks.Size = new System.Drawing.Size(171, 17);
            this.checkBoxHideHiddenFirstTracks.TabIndex = 3;
            this.checkBoxHideHiddenFirstTracks.Text = "Playlists with hidden first tracks";
            this.checkBoxHideHiddenFirstTracks.UseVisualStyleBackColor = true;
            // 
            // checkBoxHideLoops
            // 
            this.checkBoxHideLoops.AutoSize = true;
            this.checkBoxHideLoops.Location = new System.Drawing.Point(10, 67);
            this.checkBoxHideLoops.Name = "checkBoxHideLoops";
            this.checkBoxHideLoops.Size = new System.Drawing.Size(113, 17);
            this.checkBoxHideLoops.TabIndex = 2;
            this.checkBoxHideLoops.Text = "Playlists with loops";
            this.checkBoxHideLoops.UseVisualStyleBackColor = true;
            // 
            // checkBoxHideDuplicateStreamClips
            // 
            this.checkBoxHideDuplicateStreamClips.AutoSize = true;
            this.checkBoxHideDuplicateStreamClips.Location = new System.Drawing.Point(10, 44);
            this.checkBoxHideDuplicateStreamClips.Name = "checkBoxHideDuplicateStreamClips";
            this.checkBoxHideDuplicateStreamClips.Size = new System.Drawing.Size(189, 17);
            this.checkBoxHideDuplicateStreamClips.TabIndex = 1;
            this.checkBoxHideDuplicateStreamClips.Text = "Playlists with duplicate stream clips";
            this.checkBoxHideDuplicateStreamClips.UseVisualStyleBackColor = true;
            // 
            // checkBoxHideDuplicatePlaylists
            // 
            this.checkBoxHideDuplicatePlaylists.AutoSize = true;
            this.checkBoxHideDuplicatePlaylists.Location = new System.Drawing.Point(10, 20);
            this.checkBoxHideDuplicatePlaylists.Name = "checkBoxHideDuplicatePlaylists";
            this.checkBoxHideDuplicatePlaylists.Size = new System.Drawing.Size(110, 17);
            this.checkBoxHideDuplicatePlaylists.TabIndex = 0;
            this.checkBoxHideDuplicatePlaylists.Text = "Duplicate playlists";
            this.checkBoxHideDuplicatePlaylists.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(218, 316);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(137, 316);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // FormPlaylistFilter
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(303, 350);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPlaylistFilter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Playlist filter settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox checkedListBoxTypes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxMinChapterCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxMinDuration;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxHideLoops;
        private System.Windows.Forms.CheckBox checkBoxHideDuplicateStreamClips;
        private System.Windows.Forms.CheckBox checkBoxHideDuplicatePlaylists;
        private System.Windows.Forms.CheckBox checkBoxHideHiddenFirstTracks;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
    }
}