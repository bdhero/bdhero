namespace BDHero.Plugin.FileNamer
{
    partial class FormFileNamerPreferences
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
            System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup("Video", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup("Audio", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup9 = new System.Windows.Forms.ListViewGroup("Subtitles", System.Windows.Forms.HorizontalAlignment.Left);
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.selectableLabelMoviePlaceholders = new DotNetUtils.Controls.SelectableLabel();
            this.textBoxMovieFileNameExample = new DotNetUtils.Controls.SelectableLabel();
            this.textBoxMovieDirectoryExample = new DotNetUtils.Controls.SelectableLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxMovieDirectory = new DotNetUtils.Controls.FileTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxMovieFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabelTVShowReleaseDateFormat = new DotNetUtils.Controls.LinkLabel2();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxTVShowReleaseDateFormat = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.comboBoxEpisodeNumberFormat = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBoxSeasonNumberFormat = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.selectableLabelTVShowPlaceholders = new DotNetUtils.Controls.SelectableLabel();
            this.textBoxTVShowFileNameExample = new DotNetUtils.Controls.SelectableLabel();
            this.textBoxTVShowDirectoryExample = new DotNetUtils.Controls.SelectableLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxTVShowDirectory = new DotNetUtils.Controls.FileTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxTVShowFileName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxReplaceSpacesWith = new System.Windows.Forms.TextBox();
            this.checkBoxReplaceSpaces = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.listViewCodecNames = new DotNetUtils.Controls.ListView2();
            this.columnHeaderLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCodec = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonDefault = new System.Windows.Forms.Button();
            this.buttonRevert = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label6.Location = new System.Drawing.Point(61, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Example:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Placeholders:";
            // 
            // selectableLabelMoviePlaceholders
            // 
            this.selectableLabelMoviePlaceholders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectableLabelMoviePlaceholders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.selectableLabelMoviePlaceholders.Location = new System.Drawing.Point(117, 6);
            this.selectableLabelMoviePlaceholders.Name = "selectableLabelMoviePlaceholders";
            this.selectableLabelMoviePlaceholders.ReadOnly = true;
            this.selectableLabelMoviePlaceholders.Size = new System.Drawing.Size(679, 13);
            this.selectableLabelMoviePlaceholders.TabIndex = 0;
            this.selectableLabelMoviePlaceholders.Text = "%volume% %title% %year% %res% %vcodec% %acodec% %channels% %cut% %vlang% %alang%";
            // 
            // textBoxMovieFileNameExample
            // 
            this.textBoxMovieFileNameExample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMovieFileNameExample.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxMovieFileNameExample.Location = new System.Drawing.Point(117, 100);
            this.textBoxMovieFileNameExample.Name = "textBoxMovieFileNameExample";
            this.textBoxMovieFileNameExample.ReadOnly = true;
            this.textBoxMovieFileNameExample.Size = new System.Drawing.Size(679, 13);
            this.textBoxMovieFileNameExample.TabIndex = 4;
            this.textBoxMovieFileNameExample.Text = "Contact (1999) [1080p] [TrueHD 7.1].mkv";
            // 
            // textBoxMovieDirectoryExample
            // 
            this.textBoxMovieDirectoryExample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMovieDirectoryExample.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxMovieDirectoryExample.Location = new System.Drawing.Point(117, 55);
            this.textBoxMovieDirectoryExample.Name = "textBoxMovieDirectoryExample";
            this.textBoxMovieDirectoryExample.ReadOnly = true;
            this.textBoxMovieDirectoryExample.Size = new System.Drawing.Size(676, 13);
            this.textBoxMovieDirectoryExample.TabIndex = 2;
            this.textBoxMovieDirectoryExample.Text = "C:\\temp\\CONTACT";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label5.Location = new System.Drawing.Point(61, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Example:";
            // 
            // textBoxMovieDirectory
            // 
            this.textBoxMovieDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMovieDirectory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBoxMovieDirectory.DialogTitle = null;
            this.textBoxMovieDirectory.DialogType = DotNetUtils.Controls.DialogType.OpenDirectory;
            this.textBoxMovieDirectory.FileExtensions = null;
            this.textBoxMovieDirectory.Location = new System.Drawing.Point(117, 25);
            this.textBoxMovieDirectory.Name = "textBoxMovieDirectory";
            this.textBoxMovieDirectory.OverwritePrompt = false;
            this.textBoxMovieDirectory.SelectedPath = "%TEMP%\\%volume%";
            this.textBoxMovieDirectory.Size = new System.Drawing.Size(679, 24);
            this.textBoxMovieDirectory.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(766, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = ".mkv";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "File name template:";
            // 
            // textBoxMovieFileName
            // 
            this.textBoxMovieFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMovieFileName.Location = new System.Drawing.Point(117, 74);
            this.textBoxMovieFileName.Name = "textBoxMovieFileName";
            this.textBoxMovieFileName.Size = new System.Drawing.Size(643, 20);
            this.textBoxMovieFileName.TabIndex = 3;
            this.textBoxMovieFileName.Text = "%title% (%year%) [%res%] [%acodec% %channels%]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Directory template:";
            // 
            // linkLabelTVShowReleaseDateFormat
            // 
            this.linkLabelTVShowReleaseDateFormat.DisabledColor = System.Drawing.Color.Empty;
            this.linkLabelTVShowReleaseDateFormat.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelTVShowReleaseDateFormat.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelTVShowReleaseDateFormat.Location = new System.Drawing.Point(240, 178);
            this.linkLabelTVShowReleaseDateFormat.Name = "linkLabelTVShowReleaseDateFormat";
            this.linkLabelTVShowReleaseDateFormat.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelTVShowReleaseDateFormat.Size = new System.Drawing.Size(130, 14);
            this.linkLabelTVShowReleaseDateFormat.TabIndex = 8;
            this.linkLabelTVShowReleaseDateFormat.Text = "Formatting help on MSDN";
            this.linkLabelTVShowReleaseDateFormat.Click += new System.EventHandler(this.linkLabelTVShowReleaseDateFormat_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 178);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(105, 13);
            this.label15.TabIndex = 15;
            this.label15.Text = "Release date format:";
            // 
            // textBoxTVShowReleaseDateFormat
            // 
            this.textBoxTVShowReleaseDateFormat.Location = new System.Drawing.Point(117, 175);
            this.textBoxTVShowReleaseDateFormat.Name = "textBoxTVShowReleaseDateFormat";
            this.textBoxTVShowReleaseDateFormat.Size = new System.Drawing.Size(117, 20);
            this.textBoxTVShowReleaseDateFormat.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(21, 150);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(90, 13);
            this.label14.TabIndex = 13;
            this.label14.Text = "Episode # format:";
            // 
            // comboBoxEpisodeNumberFormat
            // 
            this.comboBoxEpisodeNumberFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEpisodeNumberFormat.FormattingEnabled = true;
            this.comboBoxEpisodeNumberFormat.Items.AddRange(new object[] {
            "1",
            "01"});
            this.comboBoxEpisodeNumberFormat.Location = new System.Drawing.Point(117, 147);
            this.comboBoxEpisodeNumberFormat.Name = "comboBoxEpisodeNumberFormat";
            this.comboBoxEpisodeNumberFormat.Size = new System.Drawing.Size(43, 21);
            this.comboBoxEpisodeNumberFormat.TabIndex = 6;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(23, 123);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(88, 13);
            this.label13.TabIndex = 11;
            this.label13.Text = "Season # format:";
            // 
            // comboBoxSeasonNumberFormat
            // 
            this.comboBoxSeasonNumberFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSeasonNumberFormat.FormattingEnabled = true;
            this.comboBoxSeasonNumberFormat.Items.AddRange(new object[] {
            "1",
            "01"});
            this.comboBoxSeasonNumberFormat.Location = new System.Drawing.Point(117, 120);
            this.comboBoxSeasonNumberFormat.Name = "comboBoxSeasonNumberFormat";
            this.comboBoxSeasonNumberFormat.Size = new System.Drawing.Size(43, 21);
            this.comboBoxSeasonNumberFormat.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label7.Location = new System.Drawing.Point(61, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Example:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(40, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Placeholders:";
            // 
            // selectableLabelTVShowPlaceholders
            // 
            this.selectableLabelTVShowPlaceholders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectableLabelTVShowPlaceholders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.selectableLabelTVShowPlaceholders.Location = new System.Drawing.Point(117, 6);
            this.selectableLabelTVShowPlaceholders.Name = "selectableLabelTVShowPlaceholders";
            this.selectableLabelTVShowPlaceholders.ReadOnly = true;
            this.selectableLabelTVShowPlaceholders.Size = new System.Drawing.Size(679, 13);
            this.selectableLabelTVShowPlaceholders.TabIndex = 0;
            this.selectableLabelTVShowPlaceholders.Text = "%volume% %title% %date% %res% %vcodec% %acodec% %channels% %cut% %vlang% %alang% " +
    "%episodetitle% %season% %episode%";
            // 
            // textBoxTVShowFileNameExample
            // 
            this.textBoxTVShowFileNameExample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTVShowFileNameExample.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxTVShowFileNameExample.Location = new System.Drawing.Point(117, 100);
            this.textBoxTVShowFileNameExample.Name = "textBoxTVShowFileNameExample";
            this.textBoxTVShowFileNameExample.ReadOnly = true;
            this.textBoxTVShowFileNameExample.Size = new System.Drawing.Size(679, 13);
            this.textBoxTVShowFileNameExample.TabIndex = 4;
            this.textBoxTVShowFileNameExample.Text = "s01e01 - \"My First Day\" (2001-10-02) [1080p] [DTS-HD MA 5.1].mkv";
            // 
            // textBoxTVShowDirectoryExample
            // 
            this.textBoxTVShowDirectoryExample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTVShowDirectoryExample.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxTVShowDirectoryExample.Location = new System.Drawing.Point(117, 55);
            this.textBoxTVShowDirectoryExample.Name = "textBoxTVShowDirectoryExample";
            this.textBoxTVShowDirectoryExample.ReadOnly = true;
            this.textBoxTVShowDirectoryExample.Size = new System.Drawing.Size(676, 13);
            this.textBoxTVShowDirectoryExample.TabIndex = 2;
            this.textBoxTVShowDirectoryExample.Text = "C:\\temp\\SCRUBS";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label9.Location = new System.Drawing.Point(61, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Example:";
            // 
            // textBoxTVShowDirectory
            // 
            this.textBoxTVShowDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTVShowDirectory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBoxTVShowDirectory.DialogTitle = null;
            this.textBoxTVShowDirectory.DialogType = DotNetUtils.Controls.DialogType.OpenDirectory;
            this.textBoxTVShowDirectory.FileExtensions = null;
            this.textBoxTVShowDirectory.Location = new System.Drawing.Point(117, 25);
            this.textBoxTVShowDirectory.Name = "textBoxTVShowDirectory";
            this.textBoxTVShowDirectory.OverwritePrompt = false;
            this.textBoxTVShowDirectory.SelectedPath = "%TEMP%\\%volume%";
            this.textBoxTVShowDirectory.Size = new System.Drawing.Size(679, 24);
            this.textBoxTVShowDirectory.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(766, 77);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(30, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = ".mkv";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 77);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(98, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "File name template:";
            // 
            // textBoxTVShowFileName
            // 
            this.textBoxTVShowFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTVShowFileName.Location = new System.Drawing.Point(117, 74);
            this.textBoxTVShowFileName.Name = "textBoxTVShowFileName";
            this.textBoxTVShowFileName.Size = new System.Drawing.Size(643, 20);
            this.textBoxTVShowFileName.TabIndex = 3;
            this.textBoxTVShowFileName.Text = "s%season%e%episode% - %title% (%date%) [%res%] [%acodec% %channels%]";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 32);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Directory template:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.textBoxReplaceSpacesWith);
            this.groupBox3.Controls.Add(this.checkBoxReplaceSpaces);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(810, 50);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "General";
            // 
            // textBoxReplaceSpacesWith
            // 
            this.textBoxReplaceSpacesWith.Location = new System.Drawing.Point(199, 18);
            this.textBoxReplaceSpacesWith.MaxLength = 3;
            this.textBoxReplaceSpacesWith.Name = "textBoxReplaceSpacesWith";
            this.textBoxReplaceSpacesWith.Size = new System.Drawing.Size(40, 20);
            this.textBoxReplaceSpacesWith.TabIndex = 1;
            // 
            // checkBoxReplaceSpaces
            // 
            this.checkBoxReplaceSpaces.AutoSize = true;
            this.checkBoxReplaceSpaces.Location = new System.Drawing.Point(12, 20);
            this.checkBoxReplaceSpaces.Name = "checkBoxReplaceSpaces";
            this.checkBoxReplaceSpaces.Size = new System.Drawing.Size(181, 17);
            this.checkBoxReplaceSpaces.TabIndex = 0;
            this.checkBoxReplaceSpaces.Text = "&Replace spaces in filename with:";
            this.checkBoxReplaceSpaces.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(747, 326);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(666, 326);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "&Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // listViewCodecNames
            // 
            this.listViewCodecNames.AllowColumnReorder = true;
            this.listViewCodecNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewCodecNames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderLabel,
            this.columnHeaderCodec,
            this.columnHeaderNumber});
            this.listViewCodecNames.FullRowSelect = true;
            this.listViewCodecNames.GridLines = true;
            listViewGroup7.Header = "Video";
            listViewGroup7.Name = "listViewGroupVideo";
            listViewGroup8.Header = "Audio";
            listViewGroup8.Name = "listViewGroupAudio";
            listViewGroup9.Header = "Subtitles";
            listViewGroup9.Name = "listViewGroupSubtitles";
            this.listViewCodecNames.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup7,
            listViewGroup8,
            listViewGroup9});
            this.listViewCodecNames.HideSelection = false;
            this.listViewCodecNames.LabelEdit = true;
            this.listViewCodecNames.Location = new System.Drawing.Point(6, 6);
            this.listViewCodecNames.MultiSelect = false;
            this.listViewCodecNames.Name = "listViewCodecNames";
            this.listViewCodecNames.ShowItemToolTips = true;
            this.listViewCodecNames.Size = new System.Drawing.Size(790, 214);
            this.listViewCodecNames.TabIndex = 0;
            this.listViewCodecNames.UseCompatibleStateImageBehavior = false;
            this.listViewCodecNames.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderLabel
            // 
            this.columnHeaderLabel.DisplayIndex = 2;
            this.columnHeaderLabel.Text = "Label";
            this.columnHeaderLabel.Width = 664;
            // 
            // columnHeaderCodec
            // 
            this.columnHeaderCodec.Text = "Codec";
            // 
            // columnHeaderNumber
            // 
            this.columnHeaderNumber.DisplayIndex = 0;
            this.columnHeaderNumber.Text = "#";
            this.columnHeaderNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // buttonDefault
            // 
            this.buttonDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDefault.Location = new System.Drawing.Point(93, 326);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(75, 23);
            this.buttonDefault.TabIndex = 3;
            this.buttonDefault.Text = "&Default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // buttonRevert
            // 
            this.buttonRevert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRevert.Location = new System.Drawing.Point(12, 326);
            this.buttonRevert.Name = "buttonRevert";
            this.buttonRevert.Size = new System.Drawing.Size(75, 23);
            this.buttonRevert.TabIndex = 2;
            this.buttonRevert.Text = "Re&vert";
            this.buttonRevert.UseVisualStyleBackColor = true;
            this.buttonRevert.Click += new System.EventHandler(this.buttonRevert_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 68);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(810, 252);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.selectableLabelMoviePlaceholders);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textBoxMovieFileNameExample);
            this.tabPage1.Controls.Add(this.textBoxMovieFileName);
            this.tabPage1.Controls.Add(this.textBoxMovieDirectoryExample);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.textBoxMovieDirectory);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(802, 226);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Movies";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.linkLabelTVShowReleaseDateFormat);
            this.tabPage2.Controls.Add(this.label15);
            this.tabPage2.Controls.Add(this.selectableLabelTVShowPlaceholders);
            this.tabPage2.Controls.Add(this.textBoxTVShowReleaseDateFormat);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Controls.Add(this.textBoxTVShowFileName);
            this.tabPage2.Controls.Add(this.comboBoxEpisodeNumberFormat);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.comboBoxSeasonNumberFormat);
            this.tabPage2.Controls.Add(this.textBoxTVShowDirectory);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.textBoxTVShowDirectoryExample);
            this.tabPage2.Controls.Add(this.textBoxTVShowFileNameExample);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(802, 226);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "TV Shows";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.listViewCodecNames);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(802, 226);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Codecs";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // FormFileNamerPreferences
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(834, 361);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonRevert);
            this.Controls.Add(this.buttonDefault);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox3);
            this.MinimumSize = new System.Drawing.Size(850, 400);
            this.Name = "FormFileNamerPreferences";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BDHero File Namer Preferences";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DotNetUtils.Controls.FileTextBox textBoxMovieDirectory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxMovieFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private DotNetUtils.Controls.SelectableLabel textBoxMovieFileNameExample;
        private DotNetUtils.Controls.SelectableLabel textBoxMovieDirectoryExample;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private DotNetUtils.Controls.SelectableLabel selectableLabelMoviePlaceholders;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private DotNetUtils.Controls.SelectableLabel selectableLabelTVShowPlaceholders;
        private DotNetUtils.Controls.SelectableLabel textBoxTVShowFileNameExample;
        private DotNetUtils.Controls.SelectableLabel textBoxTVShowDirectoryExample;
        private System.Windows.Forms.Label label9;
        private DotNetUtils.Controls.FileTextBox textBoxTVShowDirectory;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxTVShowFileName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBoxEpisodeNumberFormat;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBoxSeasonNumberFormat;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxReplaceSpacesWith;
        private System.Windows.Forms.CheckBox checkBoxReplaceSpaces;
        private DotNetUtils.Controls.ListView2 listViewCodecNames;
        private System.Windows.Forms.ColumnHeader columnHeaderLabel;
        private System.Windows.Forms.ColumnHeader columnHeaderCodec;
        private System.Windows.Forms.ColumnHeader columnHeaderNumber;
        private DotNetUtils.Controls.LinkLabel2 linkLabelTVShowReleaseDateFormat;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxTVShowReleaseDateFormat;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.Button buttonRevert;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
    }
}