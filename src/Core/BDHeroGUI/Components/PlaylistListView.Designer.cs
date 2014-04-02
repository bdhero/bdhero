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
    partial class PlaylistListView
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "00009.MPLS",
            "02:24:19:651",
            "16",
            "37,233,334,272",
            "SpecialFeature",
            "Theatrical",
            "English",
            "Bogus, Low Quality"}, -1);
            this.listView = new UILib.WinForms.Controls.ListView2();
            this.columnHeaderFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderChapterCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderFileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderVideoLanguage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderWarnings = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkBoxShowAllPlaylists = new System.Windows.Forms.CheckBox();
            this.linkLabelShowFilterWindow = new UILib.WinForms.Controls.LinkLabel2();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.AllowColumnReorder = true;
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFileName,
            this.columnHeaderLength,
            this.columnHeaderChapterCount,
            this.columnHeaderFileSize,
            this.columnHeaderType,
            this.columnHeaderCut,
            this.columnHeaderVideoLanguage,
            this.columnHeaderWarnings});
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listView.Location = new System.Drawing.Point(0, 16);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.ShowItemToolTips = true;
            this.listView.Size = new System.Drawing.Size(657, 88);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderFileName
            // 
            this.columnHeaderFileName.Text = "Name";
            this.columnHeaderFileName.Width = 72;
            // 
            // columnHeaderLength
            // 
            this.columnHeaderLength.Text = "Duration";
            this.columnHeaderLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderLength.Width = 75;
            // 
            // columnHeaderChapterCount
            // 
            this.columnHeaderChapterCount.Text = "Chapters";
            this.columnHeaderChapterCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderChapterCount.Width = 56;
            // 
            // columnHeaderFileSize
            // 
            this.columnHeaderFileSize.Text = "Size (bytes)";
            this.columnHeaderFileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderFileSize.Width = 87;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Type";
            this.columnHeaderType.Width = 83;
            // 
            // columnHeaderCut
            // 
            this.columnHeaderCut.Text = "Cut";
            this.columnHeaderCut.Width = 59;
            // 
            // columnHeaderVideoLanguage
            // 
            this.columnHeaderVideoLanguage.Text = "Video Language";
            this.columnHeaderVideoLanguage.Width = 93;
            // 
            // columnHeaderWarnings
            // 
            this.columnHeaderWarnings.Text = "Warnings";
            this.columnHeaderWarnings.Width = 126;
            // 
            // checkBoxShowAllPlaylists
            // 
            this.checkBoxShowAllPlaylists.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowAllPlaylists.AutoSize = true;
            this.checkBoxShowAllPlaylists.Location = new System.Drawing.Point(3, 110);
            this.checkBoxShowAllPlaylists.Name = "checkBoxShowAllPlaylists";
            this.checkBoxShowAllPlaylists.Size = new System.Drawing.Size(66, 17);
            this.checkBoxShowAllPlaylists.TabIndex = 3;
            this.checkBoxShowAllPlaylists.Text = "Show &all";
            this.checkBoxShowAllPlaylists.UseVisualStyleBackColor = true;
            this.checkBoxShowAllPlaylists.CheckedChanged += new System.EventHandler(this.checkBoxShowAllPlaylists_CheckedChanged);
            // 
            // linkLabelShowFilterWindow
            // 
            this.linkLabelShowFilterWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelShowFilterWindow.DisabledColor = System.Drawing.Color.Empty;
            this.linkLabelShowFilterWindow.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelShowFilterWindow.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelShowFilterWindow.Location = new System.Drawing.Point(75, 111);
            this.linkLabelShowFilterWindow.Name = "linkLabelShowFilterWindow";
            this.linkLabelShowFilterWindow.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelShowFilterWindow.Size = new System.Drawing.Size(39, 14);
            this.linkLabelShowFilterWindow.TabIndex = 4;
            this.linkLabelShowFilterWindow.Text = "Filter...";
            this.linkLabelShowFilterWindow.Visible = false;
            this.linkLabelShowFilterWindow.Click += new System.EventHandler(this.linkLabelShowFilterWindow_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Playlists:";
            // 
            // PlaylistListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBoxShowAllPlaylists);
            this.Controls.Add(this.linkLabelShowFilterWindow);
            this.Controls.Add(this.listView);
            this.Name = "PlaylistListView";
            this.Size = new System.Drawing.Size(657, 131);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListView2 listView;
        private System.Windows.Forms.ColumnHeader columnHeaderFileName;
        private System.Windows.Forms.ColumnHeader columnHeaderLength;
        private System.Windows.Forms.ColumnHeader columnHeaderChapterCount;
        private System.Windows.Forms.ColumnHeader columnHeaderFileSize;
        private System.Windows.Forms.ColumnHeader columnHeaderVideoLanguage;
        private System.Windows.Forms.ColumnHeader columnHeaderCut;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderWarnings;
        private System.Windows.Forms.CheckBox checkBoxShowAllPlaylists;
        private LinkLabel2 linkLabelShowFilterWindow;
        private System.Windows.Forms.Label label1;

    }
}
