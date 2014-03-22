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

using UILib.WinForms.Controls;

namespace BDHeroGUI.Components
{
    partial class TracksPanel
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
            this.splitContainerOuter = new SplitContainerWithDivider();
            this.splitContainerInner = new SplitContainerWithDivider();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.videoTrackListView = new BDHeroGUI.Components.VideoTrackListView();
            this.audioTrackListView = new BDHeroGUI.Components.AudioTrackListView();
            this.subtitleTrackListView = new BDHeroGUI.Components.SubtitleTrackListView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOuter)).BeginInit();
            this.splitContainerOuter.Panel1.SuspendLayout();
            this.splitContainerOuter.Panel2.SuspendLayout();
            this.splitContainerOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInner)).BeginInit();
            this.splitContainerInner.Panel1.SuspendLayout();
            this.splitContainerInner.Panel2.SuspendLayout();
            this.splitContainerInner.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerOuter
            // 
            this.splitContainerOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOuter.Location = new System.Drawing.Point(0, 0);
            this.splitContainerOuter.Name = "splitContainerOuter";
            // 
            // splitContainerOuter.Panel1
            // 
            this.splitContainerOuter.Panel1.Controls.Add(this.splitContainerInner);
            // 
            // splitContainerOuter.Panel2
            // 
            this.splitContainerOuter.Panel2.Controls.Add(this.subtitleTrackListView);
            this.splitContainerOuter.Panel2.Controls.Add(this.label3);
            this.splitContainerOuter.Size = new System.Drawing.Size(589, 301);
            this.splitContainerOuter.SplitterDistance = 362;
            this.splitContainerOuter.TabIndex = 0;
            // 
            // splitContainerInner
            // 
            this.splitContainerInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerInner.Location = new System.Drawing.Point(0, 0);
            this.splitContainerInner.Name = "splitContainerInner";
            this.splitContainerInner.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerInner.Panel1
            // 
            this.splitContainerInner.Panel1.Controls.Add(this.label1);
            this.splitContainerInner.Panel1.Controls.Add(this.videoTrackListView);
            // 
            // splitContainerInner.Panel2
            // 
            this.splitContainerInner.Panel2.Controls.Add(this.audioTrackListView);
            this.splitContainerInner.Panel2.Controls.Add(this.label2);
            this.splitContainerInner.Size = new System.Drawing.Size(362, 301);
            this.splitContainerInner.SplitterDistance = 96;
            this.splitContainerInner.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Subtitle Tracks:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Audio Tracks:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Video Tracks:";
            // 
            // videoTrackListView
            // 
            this.videoTrackListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoTrackListView.Location = new System.Drawing.Point(3, 16);
            this.videoTrackListView.Name = "videoTrackListView";
            this.videoTrackListView.Size = new System.Drawing.Size(356, 77);
            this.videoTrackListView.TabIndex = 5;
            // 
            // audioTrackListView
            // 
            this.audioTrackListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.audioTrackListView.Location = new System.Drawing.Point(3, 16);
            this.audioTrackListView.Name = "audioTrackListView";
            this.audioTrackListView.Size = new System.Drawing.Size(356, 182);
            this.audioTrackListView.TabIndex = 2;
            // 
            // subtitleTrackListView
            // 
            this.subtitleTrackListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subtitleTrackListView.Location = new System.Drawing.Point(6, 16);
            this.subtitleTrackListView.Name = "subtitleTrackListView";
            this.subtitleTrackListView.Size = new System.Drawing.Size(214, 282);
            this.subtitleTrackListView.TabIndex = 6;
            // 
            // TracksPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerOuter);
            this.Name = "TracksPanel";
            this.Size = new System.Drawing.Size(589, 301);
            this.splitContainerOuter.Panel1.ResumeLayout(false);
            this.splitContainerOuter.Panel2.ResumeLayout(false);
            this.splitContainerOuter.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOuter)).EndInit();
            this.splitContainerOuter.ResumeLayout(false);
            this.splitContainerInner.Panel1.ResumeLayout(false);
            this.splitContainerInner.Panel1.PerformLayout();
            this.splitContainerInner.Panel2.ResumeLayout(false);
            this.splitContainerInner.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInner)).EndInit();
            this.splitContainerInner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainerWithDivider splitContainerOuter;
        private SplitContainerWithDivider splitContainerInner;
        private System.Windows.Forms.Label label1;
        private VideoTrackListView videoTrackListView;
        private AudioTrackListView audioTrackListView;
        private System.Windows.Forms.Label label2;
        private SubtitleTrackListView subtitleTrackListView;
        private System.Windows.Forms.Label label3;

    }
}
