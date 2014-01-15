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
    partial class FormDiscInfo
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
            this.label1 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelQuickSummary = new DotNetUtils.Controls.SelectableLabel();
            this.groupBoxJacket = new System.Windows.Forms.GroupBox();
            this.pictureBoxJacket = new System.Windows.Forms.PictureBox();
            this.contextMenuStripJacket = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemJacketImagePath = new System.Windows.Forms.ToolStripMenuItem();
            this.showInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyPathToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.discInfoMetadataPanel = new BDHeroGUI.Components.DiscInfoMetadataPanel();
            this.discInfoFeaturesPanel = new BDHeroGUI.Components.DiscInfoFeaturesPanel();
            this.groupBoxJacket.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxJacket)).BeginInit();
            this.contextMenuStripJacket.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "BD-ROM:";
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(1030, 476);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelQuickSummary
            // 
            this.labelQuickSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelQuickSummary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labelQuickSummary.Location = new System.Drawing.Point(74, 13);
            this.labelQuickSummary.Name = "labelQuickSummary";
            this.labelQuickSummary.ReadOnly = true;
            this.labelQuickSummary.Size = new System.Drawing.Size(1031, 13);
            this.labelQuickSummary.TabIndex = 0;
            this.labelQuickSummary.Text = "E:\\";
            // 
            // groupBoxJacket
            // 
            this.groupBoxJacket.Controls.Add(this.pictureBoxJacket);
            this.groupBoxJacket.Location = new System.Drawing.Point(12, 225);
            this.groupBoxJacket.Name = "groupBoxJacket";
            this.groupBoxJacket.Size = new System.Drawing.Size(134, 85);
            this.groupBoxJacket.TabIndex = 4;
            this.groupBoxJacket.TabStop = false;
            this.groupBoxJacket.Text = "Jacket Image";
            // 
            // pictureBoxJacket
            // 
            this.pictureBoxJacket.ContextMenuStrip = this.contextMenuStripJacket;
            this.pictureBoxJacket.Location = new System.Drawing.Point(6, 19);
            this.pictureBoxJacket.Name = "pictureBoxJacket";
            this.pictureBoxJacket.Size = new System.Drawing.Size(104, 60);
            this.pictureBoxJacket.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxJacket.TabIndex = 4;
            this.pictureBoxJacket.TabStop = false;
            this.pictureBoxJacket.MouseEnter += new System.EventHandler(this.pictureBoxJacket_MouseEnter);
            this.pictureBoxJacket.MouseLeave += new System.EventHandler(this.pictureBoxJacket_MouseLeave);
            // 
            // contextMenuStripJacket
            // 
            this.contextMenuStripJacket.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemJacketImagePath,
            this.showInExplorerToolStripMenuItem,
            this.copyPathToClipboardToolStripMenuItem});
            this.contextMenuStripJacket.Name = "contextMenuStripJacket";
            this.contextMenuStripJacket.Size = new System.Drawing.Size(283, 92);
            this.contextMenuStripJacket.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.contextMenuStripJacket_Closed);
            this.contextMenuStripJacket.Opened += new System.EventHandler(this.contextMenuStripJacket_Opened);
            // 
            // toolStripMenuItemJacketImagePath
            // 
            this.toolStripMenuItemJacketImagePath.Enabled = false;
            this.toolStripMenuItemJacketImagePath.Name = "toolStripMenuItemJacketImagePath";
            this.toolStripMenuItemJacketImagePath.Size = new System.Drawing.Size(282, 22);
            this.toolStripMenuItemJacketImagePath.Text = "D:\\BDMV\\META\\DL\\Quack_JacketB.jpg";
            // 
            // showInExplorerToolStripMenuItem
            // 
            this.showInExplorerToolStripMenuItem.Image = global::BDHeroGUI.Properties.Resources.folder_open;
            this.showInExplorerToolStripMenuItem.Name = "showInExplorerToolStripMenuItem";
            this.showInExplorerToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.showInExplorerToolStripMenuItem.Text = "&Show in folder";
            this.showInExplorerToolStripMenuItem.Click += new System.EventHandler(this.showInExplorerToolStripMenuItem_Click);
            // 
            // copyPathToClipboardToolStripMenuItem
            // 
            this.copyPathToClipboardToolStripMenuItem.Image = global::BDHeroGUI.Properties.Resources.clipboard_arrow;
            this.copyPathToClipboardToolStripMenuItem.Name = "copyPathToClipboardToolStripMenuItem";
            this.copyPathToClipboardToolStripMenuItem.Size = new System.Drawing.Size(282, 22);
            this.copyPathToClipboardToolStripMenuItem.Text = "&Copy path to clipboard";
            this.copyPathToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyPathToClipboardToolStripMenuItem_Click);
            // 
            // discInfoMetadataPanel
            // 
            this.discInfoMetadataPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.discInfoMetadataPanel.Location = new System.Drawing.Point(152, 42);
            this.discInfoMetadataPanel.Name = "discInfoMetadataPanel";
            this.discInfoMetadataPanel.Size = new System.Drawing.Size(953, 428);
            this.discInfoMetadataPanel.TabIndex = 1;
            // 
            // discInfoFeaturesPanel
            // 
            this.discInfoFeaturesPanel.Location = new System.Drawing.Point(12, 42);
            this.discInfoFeaturesPanel.Name = "discInfoFeaturesPanel";
            this.discInfoFeaturesPanel.Size = new System.Drawing.Size(134, 177);
            this.discInfoFeaturesPanel.TabIndex = 0;
            // 
            // FormDiscInfo
            // 
            this.AcceptButton = this.buttonClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(1117, 511);
            this.Controls.Add(this.groupBoxJacket);
            this.Controls.Add(this.labelQuickSummary);
            this.Controls.Add(this.discInfoMetadataPanel);
            this.Controls.Add(this.discInfoFeaturesPanel);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.label1);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "FormDiscInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Disc Info";
            this.groupBoxJacket.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxJacket)).EndInit();
            this.contextMenuStripJacket.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Components.DiscInfoMetadataPanel discInfoMetadataPanel;
        private System.Windows.Forms.Button buttonClose;
        private Components.DiscInfoFeaturesPanel discInfoFeaturesPanel;
        private DotNetUtils.Controls.SelectableLabel labelQuickSummary;
        private System.Windows.Forms.GroupBox groupBoxJacket;
        private System.Windows.Forms.PictureBox pictureBoxJacket;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripJacket;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemJacketImagePath;
        private System.Windows.Forms.ToolStripMenuItem showInExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyPathToClipboardToolStripMenuItem;
    }
}