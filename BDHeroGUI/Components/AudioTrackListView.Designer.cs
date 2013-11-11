namespace BDHeroGUI.Components
{
    partial class AudioTrackListView
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
            this.listViewAudioTracks = new DotNetUtils.Controls.ListView2();
            this.columnHeaderCodec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderChannels = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLanguage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listViewAudioTracks
            // 
            this.listViewAudioTracks.AllowColumnReorder = true;
            this.listViewAudioTracks.CheckBoxes = true;
            this.listViewAudioTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderCodec,
            this.columnHeaderChannels,
            this.columnHeaderLanguage,
            this.columnHeaderType,
            this.columnHeaderIndex});
            this.listViewAudioTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewAudioTracks.FullRowSelect = true;
            this.listViewAudioTracks.GridLines = true;
            this.listViewAudioTracks.HideSelection = false;
            this.listViewAudioTracks.Location = new System.Drawing.Point(0, 0);
            this.listViewAudioTracks.Name = "listViewAudioTracks";
            this.listViewAudioTracks.ShowItemToolTips = true;
            this.listViewAudioTracks.Size = new System.Drawing.Size(518, 333);
            this.listViewAudioTracks.TabIndex = 0;
            this.listViewAudioTracks.UseCompatibleStateImageBehavior = false;
            this.listViewAudioTracks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderCodec
            // 
            this.columnHeaderCodec.DisplayIndex = 1;
            this.columnHeaderCodec.Text = "Codec";
            this.columnHeaderCodec.Width = 120;
            // 
            // columnHeaderChannels
            // 
            this.columnHeaderChannels.DisplayIndex = 2;
            this.columnHeaderChannels.Text = "Channels";
            this.columnHeaderChannels.Width = 80;
            // 
            // columnHeaderLanguage
            // 
            this.columnHeaderLanguage.DisplayIndex = 3;
            this.columnHeaderLanguage.Text = "Language";
            this.columnHeaderLanguage.Width = 80;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.DisplayIndex = 4;
            this.columnHeaderType.Text = "Type";
            this.columnHeaderType.Width = 209;
            // 
            // columnHeaderIndex
            // 
            this.columnHeaderIndex.DisplayIndex = 0;
            this.columnHeaderIndex.Text = "#";
            this.columnHeaderIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderIndex.Width = 25;
            // 
            // AudioTrackListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewAudioTracks);
            this.Name = "AudioTrackListView";
            this.Size = new System.Drawing.Size(518, 333);
            this.ResumeLayout(false);

        }

        #endregion

        private DotNetUtils.Controls.ListView2 listViewAudioTracks;
        private System.Windows.Forms.ColumnHeader columnHeaderCodec;
        private System.Windows.Forms.ColumnHeader columnHeaderLanguage;
        private System.Windows.Forms.ColumnHeader columnHeaderChannels;
        private System.Windows.Forms.ColumnHeader columnHeaderIndex;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
    }
}
