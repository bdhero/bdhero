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

namespace BDHeroGUI.Components
{
    partial class DiscInfoMetadataPanel
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.iconAllBdmtTitles = new System.Windows.Forms.PictureBox();
            this.iconVISAN = new System.Windows.Forms.PictureBox();
            this.iconDboxTitle = new System.Windows.Forms.PictureBox();
            this.iconAnyDVDDiscInf = new System.Windows.Forms.PictureBox();
            this.textBoxAllBdmtTitles = new System.Windows.Forms.TextBox();
            this.textBoxVISAN = new System.Windows.Forms.TextBox();
            this.textBoxDboxTitle = new System.Windows.Forms.TextBox();
            this.textBoxAnyDVDDiscInf = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxHardwareVolumeLabel = new System.Windows.Forms.TextBox();
            this.iconHardwareVolumeLabel = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxAnyDVDDiscInfSanitized = new System.Windows.Forms.TextBox();
            this.buttonValidBdmtTitles = new System.Windows.Forms.Button();
            this.buttonIsan = new System.Windows.Forms.Button();
            this.buttonDboxTitleSanitized = new System.Windows.Forms.Button();
            this.buttonVolumeLabelSanitized = new System.Windows.Forms.Button();
            this.textBoxValidBdmtTitles = new System.Windows.Forms.TextBox();
            this.textBoxIsan = new System.Windows.Forms.TextBox();
            this.textBoxDboxTitleSanitized = new System.Windows.Forms.TextBox();
            this.textBoxVolumeLabelSanitized = new System.Windows.Forms.TextBox();
            this.buttonAnyDVDDiscInfSanitized = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconAllBdmtTitles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconVISAN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconDboxTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconAnyDVDDiscInf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconHardwareVolumeLabel)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(899, 374);
            this.splitContainer1.SplitterDistance = 446;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(446, 374);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Raw Metadata";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.iconAllBdmtTitles, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.iconVISAN, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.iconDboxTitle, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.iconAnyDVDDiscInf, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxAllBdmtTitles, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.textBoxVISAN, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxDboxTitle, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxAnyDVDDiscInf, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxHardwareVolumeLabel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.iconHardwareVolumeLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 23);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(423, 338);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.AutoSize = true;
            this.panel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Location = new System.Drawing.Point(33, 192);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(120, 27);
            this.panel4.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(0, 0);
            this.label9.Margin = new System.Windows.Forms.Padding(3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 13);
            this.label9.TabIndex = 5;
            this.label9.Text = "BDMT Title(s):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label6.Location = new System.Drawing.Point(0, 11);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "META/DL/bdmt_xxx.xml";
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Location = new System.Drawing.Point(33, 63);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(81, 24);
            this.panel3.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "D-BOX Title:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label5.Location = new System.Drawing.Point(0, 11);
            this.label5.Margin = new System.Windows.Forms.Padding(3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "FilmIndex.xml";
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(33, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(96, 24);
            this.panel2.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "AnyDVD Label:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label2.Location = new System.Drawing.Point(0, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "disc.inf";
            // 
            // iconAllBdmtTitles
            // 
            this.iconAllBdmtTitles.Image = global::BDHeroGUI.Properties.Resources.tick;
            this.iconAllBdmtTitles.Location = new System.Drawing.Point(3, 192);
            this.iconAllBdmtTitles.Name = "iconAllBdmtTitles";
            this.iconAllBdmtTitles.Size = new System.Drawing.Size(24, 24);
            this.iconAllBdmtTitles.TabIndex = 14;
            this.iconAllBdmtTitles.TabStop = false;
            // 
            // iconVISAN
            // 
            this.iconVISAN.Image = global::BDHeroGUI.Properties.Resources.tick;
            this.iconVISAN.Location = new System.Drawing.Point(3, 93);
            this.iconVISAN.Name = "iconVISAN";
            this.iconVISAN.Size = new System.Drawing.Size(24, 24);
            this.iconVISAN.TabIndex = 13;
            this.iconVISAN.TabStop = false;
            // 
            // iconDboxTitle
            // 
            this.iconDboxTitle.Image = global::BDHeroGUI.Properties.Resources.tick;
            this.iconDboxTitle.Location = new System.Drawing.Point(3, 63);
            this.iconDboxTitle.Name = "iconDboxTitle";
            this.iconDboxTitle.Size = new System.Drawing.Size(24, 24);
            this.iconDboxTitle.TabIndex = 12;
            this.iconDboxTitle.TabStop = false;
            // 
            // iconAnyDVDDiscInf
            // 
            this.iconAnyDVDDiscInf.Image = global::BDHeroGUI.Properties.Resources.tick;
            this.iconAnyDVDDiscInf.Location = new System.Drawing.Point(3, 33);
            this.iconAnyDVDDiscInf.Name = "iconAnyDVDDiscInf";
            this.iconAnyDVDDiscInf.Size = new System.Drawing.Size(24, 24);
            this.iconAnyDVDDiscInf.TabIndex = 11;
            this.iconAnyDVDDiscInf.TabStop = false;
            // 
            // textBoxAllBdmtTitles
            // 
            this.textBoxAllBdmtTitles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAllBdmtTitles.Location = new System.Drawing.Point(159, 192);
            this.textBoxAllBdmtTitles.Multiline = true;
            this.textBoxAllBdmtTitles.Name = "textBoxAllBdmtTitles";
            this.textBoxAllBdmtTitles.ReadOnly = true;
            this.textBoxAllBdmtTitles.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxAllBdmtTitles.Size = new System.Drawing.Size(261, 143);
            this.textBoxAllBdmtTitles.TabIndex = 8;
            // 
            // textBoxVISAN
            // 
            this.textBoxVISAN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVISAN.Location = new System.Drawing.Point(159, 93);
            this.textBoxVISAN.Multiline = true;
            this.textBoxVISAN.Name = "textBoxVISAN";
            this.textBoxVISAN.ReadOnly = true;
            this.textBoxVISAN.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxVISAN.Size = new System.Drawing.Size(261, 93);
            this.textBoxVISAN.TabIndex = 6;
            // 
            // textBoxDboxTitle
            // 
            this.textBoxDboxTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDboxTitle.Location = new System.Drawing.Point(159, 63);
            this.textBoxDboxTitle.Name = "textBoxDboxTitle";
            this.textBoxDboxTitle.ReadOnly = true;
            this.textBoxDboxTitle.Size = new System.Drawing.Size(261, 20);
            this.textBoxDboxTitle.TabIndex = 4;
            // 
            // textBoxAnyDVDDiscInf
            // 
            this.textBoxAnyDVDDiscInf.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAnyDVDDiscInf.Location = new System.Drawing.Point(159, 33);
            this.textBoxAnyDVDDiscInf.Name = "textBoxAnyDVDDiscInf";
            this.textBoxAnyDVDDiscInf.ReadOnly = true;
            this.textBoxAnyDVDDiscInf.Size = new System.Drawing.Size(261, 20);
            this.textBoxAnyDVDDiscInf.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(33, 3);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Volume Label:";
            // 
            // textBoxHardwareVolumeLabel
            // 
            this.textBoxHardwareVolumeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHardwareVolumeLabel.Location = new System.Drawing.Point(159, 3);
            this.textBoxHardwareVolumeLabel.Name = "textBoxHardwareVolumeLabel";
            this.textBoxHardwareVolumeLabel.ReadOnly = true;
            this.textBoxHardwareVolumeLabel.Size = new System.Drawing.Size(261, 20);
            this.textBoxHardwareVolumeLabel.TabIndex = 0;
            // 
            // iconHardwareVolumeLabel
            // 
            this.iconHardwareVolumeLabel.Image = global::BDHeroGUI.Properties.Resources.tick;
            this.iconHardwareVolumeLabel.Location = new System.Drawing.Point(3, 3);
            this.iconHardwareVolumeLabel.Name = "iconHardwareVolumeLabel";
            this.iconHardwareVolumeLabel.Size = new System.Drawing.Size(24, 24);
            this.iconHardwareVolumeLabel.TabIndex = 10;
            this.iconHardwareVolumeLabel.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(33, 93);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(98, 27);
            this.panel1.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "V-ISAN / ISAN:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(0, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "AACS/mcmf.xml";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox2.Size = new System.Drawing.Size(449, 374);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Derived Metadata";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.Controls.Add(this.textBoxAnyDVDDiscInfSanitized, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.buttonValidBdmtTitles, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.buttonIsan, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.buttonDboxTitleSanitized, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.buttonVolumeLabelSanitized, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.textBoxValidBdmtTitles, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.textBoxIsan, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.textBoxDboxTitleSanitized, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.textBoxVolumeLabelSanitized, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonAnyDVDDiscInfSanitized, 2, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(10, 23);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(426, 338);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // textBoxAnyDVDDiscInfSanitized
            // 
            this.textBoxAnyDVDDiscInfSanitized.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAnyDVDDiscInfSanitized.Location = new System.Drawing.Point(3, 33);
            this.textBoxAnyDVDDiscInfSanitized.Name = "textBoxAnyDVDDiscInfSanitized";
            this.textBoxAnyDVDDiscInfSanitized.ReadOnly = true;
            this.textBoxAnyDVDDiscInfSanitized.Size = new System.Drawing.Size(360, 20);
            this.textBoxAnyDVDDiscInfSanitized.TabIndex = 2;
            // 
            // buttonValidBdmtTitles
            // 
            this.buttonValidBdmtTitles.Location = new System.Drawing.Point(369, 192);
            this.buttonValidBdmtTitles.Name = "buttonValidBdmtTitles";
            this.buttonValidBdmtTitles.Size = new System.Drawing.Size(54, 23);
            this.buttonValidBdmtTitles.TabIndex = 9;
            this.buttonValidBdmtTitles.Text = "Open...";
            this.buttonValidBdmtTitles.UseVisualStyleBackColor = true;
            // 
            // buttonIsan
            // 
            this.buttonIsan.Location = new System.Drawing.Point(369, 93);
            this.buttonIsan.Name = "buttonIsan";
            this.buttonIsan.Size = new System.Drawing.Size(54, 23);
            this.buttonIsan.TabIndex = 7;
            this.buttonIsan.Text = "Open...";
            this.buttonIsan.UseVisualStyleBackColor = true;
            // 
            // buttonDboxTitleSanitized
            // 
            this.buttonDboxTitleSanitized.Location = new System.Drawing.Point(369, 63);
            this.buttonDboxTitleSanitized.Name = "buttonDboxTitleSanitized";
            this.buttonDboxTitleSanitized.Size = new System.Drawing.Size(54, 23);
            this.buttonDboxTitleSanitized.TabIndex = 5;
            this.buttonDboxTitleSanitized.Text = "Open...";
            this.buttonDboxTitleSanitized.UseVisualStyleBackColor = true;
            // 
            // buttonVolumeLabelSanitized
            // 
            this.buttonVolumeLabelSanitized.Location = new System.Drawing.Point(369, 3);
            this.buttonVolumeLabelSanitized.Name = "buttonVolumeLabelSanitized";
            this.buttonVolumeLabelSanitized.Size = new System.Drawing.Size(54, 23);
            this.buttonVolumeLabelSanitized.TabIndex = 1;
            this.buttonVolumeLabelSanitized.Text = "Open...";
            this.buttonVolumeLabelSanitized.UseVisualStyleBackColor = true;
            // 
            // textBoxValidBdmtTitles
            // 
            this.textBoxValidBdmtTitles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxValidBdmtTitles.Location = new System.Drawing.Point(3, 192);
            this.textBoxValidBdmtTitles.Multiline = true;
            this.textBoxValidBdmtTitles.Name = "textBoxValidBdmtTitles";
            this.textBoxValidBdmtTitles.ReadOnly = true;
            this.textBoxValidBdmtTitles.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxValidBdmtTitles.Size = new System.Drawing.Size(360, 143);
            this.textBoxValidBdmtTitles.TabIndex = 8;
            // 
            // textBoxIsan
            // 
            this.textBoxIsan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxIsan.Location = new System.Drawing.Point(3, 93);
            this.textBoxIsan.Multiline = true;
            this.textBoxIsan.Name = "textBoxIsan";
            this.textBoxIsan.ReadOnly = true;
            this.textBoxIsan.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxIsan.Size = new System.Drawing.Size(360, 93);
            this.textBoxIsan.TabIndex = 6;
            // 
            // textBoxDboxTitleSanitized
            // 
            this.textBoxDboxTitleSanitized.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDboxTitleSanitized.Location = new System.Drawing.Point(3, 63);
            this.textBoxDboxTitleSanitized.Name = "textBoxDboxTitleSanitized";
            this.textBoxDboxTitleSanitized.ReadOnly = true;
            this.textBoxDboxTitleSanitized.Size = new System.Drawing.Size(360, 20);
            this.textBoxDboxTitleSanitized.TabIndex = 4;
            // 
            // textBoxVolumeLabelSanitized
            // 
            this.textBoxVolumeLabelSanitized.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVolumeLabelSanitized.Location = new System.Drawing.Point(3, 3);
            this.textBoxVolumeLabelSanitized.Name = "textBoxVolumeLabelSanitized";
            this.textBoxVolumeLabelSanitized.ReadOnly = true;
            this.textBoxVolumeLabelSanitized.Size = new System.Drawing.Size(360, 20);
            this.textBoxVolumeLabelSanitized.TabIndex = 0;
            // 
            // buttonAnyDVDDiscInfSanitized
            // 
            this.buttonAnyDVDDiscInfSanitized.Location = new System.Drawing.Point(369, 33);
            this.buttonAnyDVDDiscInfSanitized.Name = "buttonAnyDVDDiscInfSanitized";
            this.buttonAnyDVDDiscInfSanitized.Size = new System.Drawing.Size(54, 23);
            this.buttonAnyDVDDiscInfSanitized.TabIndex = 3;
            this.buttonAnyDVDDiscInfSanitized.Text = "Open...";
            this.buttonAnyDVDDiscInfSanitized.UseVisualStyleBackColor = true;
            // 
            // DiscInfoMetadataPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "DiscInfoMetadataPanel";
            this.Size = new System.Drawing.Size(899, 374);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconAllBdmtTitles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconVISAN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconDboxTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconAnyDVDDiscInf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconHardwareVolumeLabel)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxAllBdmtTitles;
        private System.Windows.Forms.TextBox textBoxVISAN;
        private System.Windows.Forms.TextBox textBoxDboxTitle;
        private System.Windows.Forms.TextBox textBoxAnyDVDDiscInf;
        private System.Windows.Forms.TextBox textBoxHardwareVolumeLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox textBoxValidBdmtTitles;
        private System.Windows.Forms.TextBox textBoxIsan;
        private System.Windows.Forms.TextBox textBoxDboxTitleSanitized;
        private System.Windows.Forms.Button buttonValidBdmtTitles;
        private System.Windows.Forms.Button buttonIsan;
        private System.Windows.Forms.Button buttonDboxTitleSanitized;
        private System.Windows.Forms.Button buttonVolumeLabelSanitized;
        private System.Windows.Forms.PictureBox iconAllBdmtTitles;
        private System.Windows.Forms.PictureBox iconVISAN;
        private System.Windows.Forms.PictureBox iconDboxTitle;
        private System.Windows.Forms.PictureBox iconAnyDVDDiscInf;
        private System.Windows.Forms.PictureBox iconHardwareVolumeLabel;
        private System.Windows.Forms.TextBox textBoxVolumeLabelSanitized;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxAnyDVDDiscInfSanitized;
        private System.Windows.Forms.Button buttonAnyDVDDiscInfSanitized;
    }
}
