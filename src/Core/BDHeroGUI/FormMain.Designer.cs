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

using TextEditor.WinForms;
using UILib.WinForms.Controls;
using UILib.WinForms.Dialogs.FS;

namespace BDHeroGUI
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.buttonScan = new System.Windows.Forms.Button();
            this.buttonCancelScan = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonCancelConvert = new System.Windows.Forms.Button();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.panelRoot = new System.Windows.Forms.Panel();
            this.linkLabelNameProviderPreferences = new UILib.WinForms.Controls.LinkLabel2();
            this.textBoxInput = new TextEditor.WinForms.SyntaxHighlightingFileTextBox();
            this.splitContainerMain = new UILib.WinForms.Controls.SplitContainerWithDivider();
            this.splitContainerTop = new UILib.WinForms.Controls.SplitContainerWithDivider();
            this.playlistListView = new BDHeroGUI.Components.PlaylistListView();
            this.mediaPanel = new BDHeroGUI.Components.MediaPanel();
            this.splitContainerWithDivider1 = new UILib.WinForms.Controls.SplitContainerWithDivider();
            this.tracksPanel = new BDHeroGUI.Components.TracksPanel();
            this.chaptersPanel = new BDHeroGUI.Components.ChaptersPanel();
            this.textBoxOutput = new TextEditor.WinForms.SyntaxHighlightingFileTextBox();
            this.progressBar = new UILib.WinForms.Controls.ProgressBar2();
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openBDROMFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDiscToolStripMenuItem = new BDHeroGUI.Components.DiscMenu(this.components);
            this.rescanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.searchForMetadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.newInstanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.discInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.filterPlaylistsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllPlaylistsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.filterTracksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllTracksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remuxerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.homepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.submitABugReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.suggestAFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogFileInWindowsExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutBDHeroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceGarbageCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new UILib.WinForms.Controls.SelectableStatusStrip();
            this.toolStripStatusLabelOffline = new System.Windows.Forms.ToolStripStatusLabel();
            this.appendDividerToLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelRoot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).BeginInit();
            this.splitContainerTop.Panel1.SuspendLayout();
            this.splitContainerTop.Panel2.SuspendLayout();
            this.splitContainerTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWithDivider1)).BeginInit();
            this.splitContainerWithDivider1.Panel1.SuspendLayout();
            this.splitContainerWithDivider1.Panel2.SuspendLayout();
            this.splitContainerWithDivider1.SuspendLayout();
            this.menuStripTop.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input BD-ROM:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 454);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Status:";
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStatus.Location = new System.Drawing.Point(3, 470);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxStatus.Size = new System.Drawing.Size(1148, 37);
            this.textBoxStatus.TabIndex = 8;
            // 
            // buttonScan
            // 
            this.buttonScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonScan.Location = new System.Drawing.Point(995, 3);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(75, 23);
            this.buttonScan.TabIndex = 1;
            this.buttonScan.Text = "Scan";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // buttonCancelScan
            // 
            this.buttonCancelScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelScan.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelScan.Location = new System.Drawing.Point(1076, 3);
            this.buttonCancelScan.Name = "buttonCancelScan";
            this.buttonCancelScan.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelScan.TabIndex = 2;
            this.buttonCancelScan.Text = "Cancel";
            this.buttonCancelScan.UseVisualStyleBackColor = true;
            this.buttonCancelScan.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 412);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Output MKV file:";
            // 
            // buttonCancelConvert
            // 
            this.buttonCancelConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancelConvert.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelConvert.Location = new System.Drawing.Point(1076, 407);
            this.buttonCancelConvert.Name = "buttonCancelConvert";
            this.buttonCancelConvert.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelConvert.TabIndex = 6;
            this.buttonCancelConvert.Text = "Cancel";
            this.buttonCancelConvert.UseVisualStyleBackColor = true;
            this.buttonCancelConvert.Click += new System.EventHandler(this.buttonCancelConvert_Click);
            // 
            // buttonConvert
            // 
            this.buttonConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConvert.Location = new System.Drawing.Point(995, 407);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(75, 23);
            this.buttonConvert.TabIndex = 5;
            this.buttonConvert.Text = "Convert";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // panelRoot
            // 
            this.panelRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRoot.Controls.Add(this.linkLabelNameProviderPreferences);
            this.panelRoot.Controls.Add(this.textBoxInput);
            this.panelRoot.Controls.Add(this.splitContainerMain);
            this.panelRoot.Controls.Add(this.label1);
            this.panelRoot.Controls.Add(this.buttonCancelConvert);
            this.panelRoot.Controls.Add(this.label3);
            this.panelRoot.Controls.Add(this.buttonConvert);
            this.panelRoot.Controls.Add(this.textBoxStatus);
            this.panelRoot.Controls.Add(this.textBoxOutput);
            this.panelRoot.Controls.Add(this.buttonScan);
            this.panelRoot.Controls.Add(this.label2);
            this.panelRoot.Controls.Add(this.progressBar);
            this.panelRoot.Controls.Add(this.buttonCancelScan);
            this.panelRoot.Location = new System.Drawing.Point(12, 27);
            this.panelRoot.Name = "panelRoot";
            this.panelRoot.Size = new System.Drawing.Size(1154, 539);
            this.panelRoot.TabIndex = 13;
            // 
            // linkLabelNameProviderPreferences
            // 
            this.linkLabelNameProviderPreferences.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabelNameProviderPreferences.DisabledColor = System.Drawing.Color.Empty;
            this.linkLabelNameProviderPreferences.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelNameProviderPreferences.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelNameProviderPreferences.Location = new System.Drawing.Point(93, 437);
            this.linkLabelNameProviderPreferences.Name = "linkLabelNameProviderPreferences";
            this.linkLabelNameProviderPreferences.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelNameProviderPreferences.Size = new System.Drawing.Size(121, 14);
            this.linkLabelNameProviderPreferences.TabIndex = 7;
            this.linkLabelNameProviderPreferences.Text = "File name preferences...";
            this.linkLabelNameProviderPreferences.Click += new System.EventHandler(this.linkLabelNameProviderPreferences_Click);
            // 
            // textBoxInput
            // 
            this.textBoxInput.AllowAnyExtension = false;
            this.textBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBoxInput.DialogTitle = "Select a BD-ROM folder:";
            this.textBoxInput.DialogType = UILib.WinForms.Dialogs.FS.FileSystemDialogType.OpenDirectory;
            this.textBoxInput.FileTypes = null;
            this.textBoxInput.Location = new System.Drawing.Point(93, 3);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.OverwritePrompt = false;
            this.textBoxInput.SelectedPath = "D:\\";
            this.textBoxInput.ShowNewFolderButton = false;
            this.textBoxInput.Size = new System.Drawing.Size(896, 24);
            this.textBoxInput.TabIndex = 0;
            this.textBoxInput.SelectedPathChanged += new System.EventHandler(this.textBoxInput_SelectedPathChanged);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMain.Location = new System.Drawing.Point(3, 32);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerTop);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerWithDivider1);
            this.splitContainerMain.Size = new System.Drawing.Size(1148, 369);
            this.splitContainerMain.SplitterDistance = 105;
            this.splitContainerMain.TabIndex = 3;
            // 
            // splitContainerTop
            // 
            this.splitContainerTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTop.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTop.Name = "splitContainerTop";
            // 
            // splitContainerTop.Panel1
            // 
            this.splitContainerTop.Panel1.Controls.Add(this.playlistListView);
            // 
            // splitContainerTop.Panel2
            // 
            this.splitContainerTop.Panel2.Controls.Add(this.mediaPanel);
            this.splitContainerTop.Size = new System.Drawing.Size(1148, 105);
            this.splitContainerTop.SplitterDistance = 711;
            this.splitContainerTop.TabIndex = 7;
            // 
            // playlistListView
            // 
            this.playlistListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playlistListView.Location = new System.Drawing.Point(0, 0);
            this.playlistListView.Name = "playlistListView";
            this.playlistListView.Playlists = null;
            this.playlistListView.SelectedPlaylist = null;
            this.playlistListView.ShowAll = false;
            this.playlistListView.Size = new System.Drawing.Size(711, 105);
            this.playlistListView.TabIndex = 1;
            // 
            // mediaPanel
            // 
            this.mediaPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaPanel.Location = new System.Drawing.Point(0, 0);
            this.mediaPanel.Name = "mediaPanel";
            this.mediaPanel.Size = new System.Drawing.Size(433, 105);
            this.mediaPanel.TabIndex = 0;
            // 
            // splitContainerWithDivider1
            // 
            this.splitContainerWithDivider1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerWithDivider1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerWithDivider1.Name = "splitContainerWithDivider1";
            // 
            // splitContainerWithDivider1.Panel1
            // 
            this.splitContainerWithDivider1.Panel1.Controls.Add(this.tracksPanel);
            // 
            // splitContainerWithDivider1.Panel2
            // 
            this.splitContainerWithDivider1.Panel2.Controls.Add(this.chaptersPanel);
            this.splitContainerWithDivider1.Size = new System.Drawing.Size(1148, 260);
            this.splitContainerWithDivider1.SplitterDistance = 791;
            this.splitContainerWithDivider1.TabIndex = 1;
            // 
            // tracksPanel
            // 
            this.tracksPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tracksPanel.Location = new System.Drawing.Point(0, 0);
            this.tracksPanel.Name = "tracksPanel";
            this.tracksPanel.ShowAll = false;
            this.tracksPanel.Size = new System.Drawing.Size(791, 260);
            this.tracksPanel.TabIndex = 0;
            // 
            // chaptersPanel
            // 
            this.chaptersPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chaptersPanel.Location = new System.Drawing.Point(0, 0);
            this.chaptersPanel.Name = "chaptersPanel";
            this.chaptersPanel.Playlist = null;
            this.chaptersPanel.Size = new System.Drawing.Size(353, 260);
            this.chaptersPanel.TabIndex = 0;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.AllowAnyExtension = false;
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBoxOutput.DialogTitle = "Save MKV file:";
            this.textBoxOutput.DialogType = UILib.WinForms.Dialogs.FS.FileSystemDialogType.SaveFile;
            this.textBoxOutput.FileTypes = null;
            this.textBoxOutput.Location = new System.Drawing.Point(93, 407);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.SelectedPath = "";
            this.textBoxOutput.Size = new System.Drawing.Size(896, 24);
            this.textBoxOutput.TabIndex = 4;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.BackColorBottom = System.Drawing.Color.Empty;
            this.progressBar.BackColorTop = System.Drawing.Color.Empty;
            this.progressBar.Location = new System.Drawing.Point(3, 513);
            this.progressBar.Maximum = 100000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1148, 23);
            this.progressBar.Step = 1;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 11;
            this.progressBar.TextOutline = true;
            this.progressBar.TextOutlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.progressBar.TextOutlineWidth = 2;
            this.progressBar.UseCustomColors = false;
            this.progressBar.ValuePercent = 0D;
            // 
            // menuStripTop
            // 
            this.menuStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.pluginsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.updateToolStripMenuItem});
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Size = new System.Drawing.Size(1178, 24);
            this.menuStripTop.TabIndex = 14;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openBDROMFolderToolStripMenuItem,
            this.openDiscToolStripMenuItem,
            this.rescanToolStripMenuItem,
            this.toolStripMenuItem5,
            this.searchForMetadataToolStripMenuItem,
            this.toolStripMenuItem4,
            this.newInstanceToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openBDROMFolderToolStripMenuItem
            // 
            this.openBDROMFolderToolStripMenuItem.Image = global::BDHeroGUI.Properties.Resources.folder_open;
            this.openBDROMFolderToolStripMenuItem.Name = "openBDROMFolderToolStripMenuItem";
            this.openBDROMFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openBDROMFolderToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.openBDROMFolderToolStripMenuItem.Text = "&Open BD-ROM Folder...";
            this.openBDROMFolderToolStripMenuItem.Click += new System.EventHandler(this.openBDROMFolderToolStripMenuItem_Click);
            // 
            // openDiscToolStripMenuItem
            // 
            this.openDiscToolStripMenuItem.Image = global::BDHeroGUI.Properties.Resources.cd;
            this.openDiscToolStripMenuItem.Name = "openDiscToolStripMenuItem";
            this.openDiscToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.openDiscToolStripMenuItem.Text = "Open &Disc";
            // 
            // rescanToolStripMenuItem
            // 
            this.rescanToolStripMenuItem.Image = global::BDHeroGUI.Properties.Resources.refresh_green;
            this.rescanToolStripMenuItem.Name = "rescanToolStripMenuItem";
            this.rescanToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.rescanToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.rescanToolStripMenuItem.Text = "&Rescan";
            this.rescanToolStripMenuItem.Click += new System.EventHandler(this.rescanToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(238, 6);
            // 
            // searchForMetadataToolStripMenuItem
            // 
            this.searchForMetadataToolStripMenuItem.Name = "searchForMetadataToolStripMenuItem";
            this.searchForMetadataToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.searchForMetadataToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.searchForMetadataToolStripMenuItem.Text = "&Search for Metadata...";
            this.searchForMetadataToolStripMenuItem.Click += new System.EventHandler(this.searchForMetadataToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(238, 6);
            // 
            // newInstanceToolStripMenuItem
            // 
            this.newInstanceToolStripMenuItem.Name = "newInstanceToolStripMenuItem";
            this.newInstanceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newInstanceToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.newInstanceToolStripMenuItem.Text = "&New Instance";
            this.newInstanceToolStripMenuItem.Click += new System.EventHandler(this.newInstanceToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.discInfoToolStripMenuItem,
            this.toolStripMenuItem7,
            this.filterPlaylistsToolStripMenuItem,
            this.showAllPlaylistsToolStripMenuItem,
            this.toolStripMenuItem6,
            this.filterTracksToolStripMenuItem,
            this.showAllTracksToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // discInfoToolStripMenuItem
            // 
            this.discInfoToolStripMenuItem.Image = global::BDHeroGUI.Properties.Resources.info;
            this.discInfoToolStripMenuItem.Name = "discInfoToolStripMenuItem";
            this.discInfoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.discInfoToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.discInfoToolStripMenuItem.Text = "Disc &Info...";
            this.discInfoToolStripMenuItem.Click += new System.EventHandler(this.discInfoToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(235, 6);
            // 
            // filterPlaylistsToolStripMenuItem
            // 
            this.filterPlaylistsToolStripMenuItem.Image = global::BDHeroGUI.Properties.Resources.filter;
            this.filterPlaylistsToolStripMenuItem.Name = "filterPlaylistsToolStripMenuItem";
            this.filterPlaylistsToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.filterPlaylistsToolStripMenuItem.Text = "Filter &Playlists...";
            this.filterPlaylistsToolStripMenuItem.Click += new System.EventHandler(this.filterPlaylistsToolStripMenuItem_Click);
            // 
            // showAllPlaylistsToolStripMenuItem
            // 
            this.showAllPlaylistsToolStripMenuItem.Name = "showAllPlaylistsToolStripMenuItem";
            this.showAllPlaylistsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
            this.showAllPlaylistsToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.showAllPlaylistsToolStripMenuItem.Text = "Show All Playlists";
            this.showAllPlaylistsToolStripMenuItem.Click += new System.EventHandler(this.showAllPlaylistsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(235, 6);
            // 
            // filterTracksToolStripMenuItem
            // 
            this.filterTracksToolStripMenuItem.Image = global::BDHeroGUI.Properties.Resources.filter;
            this.filterTracksToolStripMenuItem.Name = "filterTracksToolStripMenuItem";
            this.filterTracksToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.filterTracksToolStripMenuItem.Text = "Filter &Tracks...";
            this.filterTracksToolStripMenuItem.Click += new System.EventHandler(this.filterTracksToolStripMenuItem_Click);
            // 
            // showAllTracksToolStripMenuItem
            // 
            this.showAllTracksToolStripMenuItem.CheckOnClick = true;
            this.showAllTracksToolStripMenuItem.Name = "showAllTracksToolStripMenuItem";
            this.showAllTracksToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.T)));
            this.showAllTracksToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.showAllTracksToolStripMenuItem.Text = "Show All Tracks";
            this.showAllTracksToolStripMenuItem.Click += new System.EventHandler(this.showAllTracksToolStripMenuItem_CheckedChanged);
            // 
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.pluginsToolStripMenuItem.Text = "Plugins";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remuxerToolStripMenuItem,
            this.toolStripMenuItem1,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // remuxerToolStripMenuItem
            // 
            this.remuxerToolStripMenuItem.Enabled = false;
            this.remuxerToolStripMenuItem.Name = "remuxerToolStripMenuItem";
            this.remuxerToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.remuxerToolStripMenuItem.Text = "&Remuxer";
            this.remuxerToolStripMenuItem.ToolTipText = "Launches the Remuxer in a separate window";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(147, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Enabled = false;
            this.optionsToolStripMenuItem.Image = global::BDHeroGUI.Properties.Resources.settings;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.optionsToolStripMenuItem.Text = "&Options...";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homepageToolStripMenuItem,
            this.documentationToolStripMenuItem,
            this.submitABugReportToolStripMenuItem,
            this.suggestAFeatureToolStripMenuItem,
            this.showLogFileInWindowsExplorerToolStripMenuItem,
            this.toolStripMenuItem3,
            this.checkForUpdatesToolStripMenuItem,
            this.toolStripMenuItem2,
            this.aboutBDHeroToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // homepageToolStripMenuItem
            // 
            this.homepageToolStripMenuItem.Name = "homepageToolStripMenuItem";
            this.homepageToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.homepageToolStripMenuItem.Text = "&Homepage";
            this.homepageToolStripMenuItem.Click += new System.EventHandler(this.homepageToolStripMenuItem_Click);
            // 
            // documentationToolStripMenuItem
            // 
            this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            this.documentationToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.documentationToolStripMenuItem.Text = "Online &Documentation";
            this.documentationToolStripMenuItem.Click += new System.EventHandler(this.documentationToolStripMenuItem_Click);
            // 
            // submitABugReportToolStripMenuItem
            // 
            this.submitABugReportToolStripMenuItem.Name = "submitABugReportToolStripMenuItem";
            this.submitABugReportToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.submitABugReportToolStripMenuItem.Text = "Report a &Bug";
            this.submitABugReportToolStripMenuItem.Click += new System.EventHandler(this.submitABugReportToolStripMenuItem_Click);
            // 
            // suggestAFeatureToolStripMenuItem
            // 
            this.suggestAFeatureToolStripMenuItem.Name = "suggestAFeatureToolStripMenuItem";
            this.suggestAFeatureToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.suggestAFeatureToolStripMenuItem.Text = "Suggest a &Feature";
            this.suggestAFeatureToolStripMenuItem.Click += new System.EventHandler(this.suggestAFeatureToolStripMenuItem_Click);
            // 
            // showLogFileInWindowsExplorerToolStripMenuItem
            // 
            this.showLogFileInWindowsExplorerToolStripMenuItem.Name = "showLogFileInWindowsExplorerToolStripMenuItem";
            this.showLogFileInWindowsExplorerToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.showLogFileInWindowsExplorerToolStripMenuItem.Text = "Show &Log Files";
            this.showLogFileInWindowsExplorerToolStripMenuItem.Click += new System.EventHandler(this.showLogFileInWindowsExplorerToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(192, 6);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for &Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(192, 6);
            // 
            // aboutBDHeroToolStripMenuItem
            // 
            this.aboutBDHeroToolStripMenuItem.Name = "aboutBDHeroToolStripMenuItem";
            this.aboutBDHeroToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutBDHeroToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.aboutBDHeroToolStripMenuItem.Text = "&About BDHero";
            this.aboutBDHeroToolStripMenuItem.Click += new System.EventHandler(this.aboutBDHeroToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.forceGarbageCollectionToolStripMenuItem,
            this.appendDividerToLogToolStripMenuItem});
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // forceGarbageCollectionToolStripMenuItem
            // 
            this.forceGarbageCollectionToolStripMenuItem.Name = "forceGarbageCollectionToolStripMenuItem";
            this.forceGarbageCollectionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.G)));
            this.forceGarbageCollectionToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.forceGarbageCollectionToolStripMenuItem.Text = "Force &Garbage Collection";
            this.forceGarbageCollectionToolStripMenuItem.Click += new System.EventHandler(this.forceGarbageCollectionToolStripMenuItem_Click);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadUpdateToolStripMenuItem});
            this.updateToolStripMenuItem.Image = global::BDHeroGUI.Properties.Resources.asterisk_orange;
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.updateToolStripMenuItem.Text = "&Update";
            // 
            // downloadUpdateToolStripMenuItem
            // 
            this.downloadUpdateToolStripMenuItem.Name = "downloadUpdateToolStripMenuItem";
            this.downloadUpdateToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.downloadUpdateToolStripMenuItem.Text = "Download v0.0.0.0";
            this.downloadUpdateToolStripMenuItem.Click += new System.EventHandler(this.downloadUpdateToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.CanTabInto = false;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelOffline});
            this.statusStrip1.Location = new System.Drawing.Point(0, 579);
            this.statusStrip1.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1178, 22);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.TabStop = true;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelOffline
            // 
            this.toolStripStatusLabelOffline.Image = global::BDHeroGUI.Properties.Resources.warning;
            this.toolStripStatusLabelOffline.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripStatusLabelOffline.Name = "toolStripStatusLabelOffline";
            this.toolStripStatusLabelOffline.Size = new System.Drawing.Size(257, 17);
            this.toolStripStatusLabelOffline.Text = "Offline: metadata search may be unavailable";
            // 
            // appendDividerToLogToolStripMenuItem
            // 
            this.appendDividerToLogToolStripMenuItem.Name = "appendDividerToLogToolStripMenuItem";
            this.appendDividerToLogToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.L)));
            this.appendDividerToLogToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.appendDividerToLogToolStripMenuItem.Text = "Append Divider to &Log";
            this.appendDividerToLogToolStripMenuItem.Click += new System.EventHandler(this.appendDividerToLogToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AcceptButton = this.buttonScan;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancelScan;
            this.ClientSize = new System.Drawing.Size(1178, 601);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panelRoot);
            this.Controls.Add(this.menuStripTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripTop;
            this.Name = "FormMain";
            this.Text = "BDHero GUI";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
            this.DragLeave += new System.EventHandler(this.FormMain_DragLeave);
            this.panelRoot.ResumeLayout(false);
            this.panelRoot.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerTop.Panel1.ResumeLayout(false);
            this.splitContainerTop.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTop)).EndInit();
            this.splitContainerTop.ResumeLayout(false);
            this.splitContainerWithDivider1.Panel1.ResumeLayout(false);
            this.splitContainerWithDivider1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerWithDivider1)).EndInit();
            this.splitContainerWithDivider1.ResumeLayout(false);
            this.menuStripTop.ResumeLayout(false);
            this.menuStripTop.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private SyntaxHighlightingFileTextBox textBoxInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Button buttonScan;
        private ProgressBar2 progressBar;
        private System.Windows.Forms.Button buttonCancelScan;
        private SyntaxHighlightingFileTextBox textBoxOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonCancelConvert;
        private System.Windows.Forms.Button buttonConvert;
        private SplitContainerWithDivider splitContainerMain;
        private Components.TracksPanel tracksPanel;
        private SplitContainerWithDivider splitContainerTop;
        private Components.PlaylistListView playlistListView;
        private Components.MediaPanel mediaPanel;
        private System.Windows.Forms.Panel panelRoot;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openBDROMFolderToolStripMenuItem;
        private BDHeroGUI.Components.DiscMenu openDiscToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remuxerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem homepageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem submitABugReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem suggestAFeatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem aboutBDHeroToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem searchForMetadataToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem showLogFileInWindowsExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rescanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem discInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem filterPlaylistsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllPlaylistsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem filterTracksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllTracksToolStripMenuItem;
        private SplitContainerWithDivider splitContainerWithDivider1;
        private Components.ChaptersPanel chaptersPanel;
        private LinkLabel2 linkLabelNameProviderPreferences;
        private SelectableStatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelOffline;
        private System.Windows.Forms.ToolStripMenuItem newInstanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceGarbageCollectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem appendDividerToLogToolStripMenuItem;
    }
}

