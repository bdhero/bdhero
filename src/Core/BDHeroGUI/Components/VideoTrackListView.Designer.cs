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

namespace BDHeroGUI.Components
{
    partial class VideoTrackListView
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
            this.listViewVideoTracks = new UILib.Controls.ListView2();
            this.columnHeaderCodec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderResolution = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFrameRate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAspectRatio = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listViewVideoTracks
            // 
            this.listViewVideoTracks.AllowColumnReorder = true;
            this.listViewVideoTracks.CheckBoxes = true;
            this.listViewVideoTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderCodec,
            this.columnHeaderResolution,
            this.columnHeaderFrameRate,
            this.columnHeaderAspectRatio,
            this.columnHeaderType,
            this.columnHeaderIndex});
            this.listViewVideoTracks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewVideoTracks.FullRowSelect = true;
            this.listViewVideoTracks.GridLines = true;
            this.listViewVideoTracks.HideSelection = false;
            this.listViewVideoTracks.Location = new System.Drawing.Point(0, 0);
            this.listViewVideoTracks.MultiSelect = false;
            this.listViewVideoTracks.Name = "listViewVideoTracks";
            this.listViewVideoTracks.ShowItemToolTips = true;
            this.listViewVideoTracks.Size = new System.Drawing.Size(518, 333);
            this.listViewVideoTracks.TabIndex = 0;
            this.listViewVideoTracks.UseCompatibleStateImageBehavior = false;
            this.listViewVideoTracks.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderCodec
            // 
            this.columnHeaderCodec.DisplayIndex = 1;
            this.columnHeaderCodec.Text = "Codec";
            this.columnHeaderCodec.Width = 80;
            // 
            // columnHeaderResolution
            // 
            this.columnHeaderResolution.DisplayIndex = 2;
            this.columnHeaderResolution.Text = "Resolution";
            this.columnHeaderResolution.Width = 80;
            // 
            // columnHeaderFrameRate
            // 
            this.columnHeaderFrameRate.DisplayIndex = 3;
            this.columnHeaderFrameRate.Text = "Frame Rate";
            this.columnHeaderFrameRate.Width = 80;
            // 
            // columnHeaderAspectRatio
            // 
            this.columnHeaderAspectRatio.DisplayIndex = 4;
            this.columnHeaderAspectRatio.Text = "Aspect Ratio";
            this.columnHeaderAspectRatio.Width = 80;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.DisplayIndex = 5;
            this.columnHeaderType.Text = "Type";
            this.columnHeaderType.Width = 169;
            // 
            // columnHeaderIndex
            // 
            this.columnHeaderIndex.DisplayIndex = 0;
            this.columnHeaderIndex.Text = "#";
            this.columnHeaderIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderIndex.Width = 25;
            // 
            // VideoTrackListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewVideoTracks);
            this.Name = "VideoTrackListView";
            this.Size = new System.Drawing.Size(518, 333);
            this.ResumeLayout(false);

        }

        #endregion

        private UILib.Controls.ListView2 listViewVideoTracks;
        private System.Windows.Forms.ColumnHeader columnHeaderCodec;
        private System.Windows.Forms.ColumnHeader columnHeaderResolution;
        private System.Windows.Forms.ColumnHeader columnHeaderFrameRate;
        private System.Windows.Forms.ColumnHeader columnHeaderAspectRatio;
        private System.Windows.Forms.ColumnHeader columnHeaderIndex;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
    }
}
