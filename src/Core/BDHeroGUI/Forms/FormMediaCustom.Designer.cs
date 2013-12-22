namespace BDHeroGUI.Forms
{
    partial class FormMediaCustom
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabelURL = new DotNetUtils.Controls.LinkLabel2();
            this.linkLabelBrowse = new DotNetUtils.Controls.LinkLabel2();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBoxPoster = new System.Windows.Forms.PictureBox();
            this.textBoxYear = new System.Windows.Forms.NumericUpDown();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTipPictureBox = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPoster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxYear)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabelURL);
            this.panel1.Controls.Add(this.linkLabelBrowse);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pictureBoxPoster);
            this.panel1.Controls.Add(this.textBoxYear);
            this.panel1.Controls.Add(this.textBoxTitle);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(434, 213);
            this.panel1.TabIndex = 0;
            // 
            // linkLabelURL
            // 
            this.linkLabelURL.DisabledColor = System.Drawing.Color.Empty;
            this.linkLabelURL.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelURL.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelURL.Location = new System.Drawing.Point(190, 104);
            this.linkLabelURL.Name = "linkLabelURL";
            this.linkLabelURL.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelURL.Size = new System.Drawing.Size(65, 14);
            this.linkLabelURL.TabIndex = 3;
            this.linkLabelURL.Text = "From URL...";
            // 
            // linkLabelBrowse
            // 
            this.linkLabelBrowse.DisabledColor = System.Drawing.Color.Empty;
            this.linkLabelBrowse.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.linkLabelBrowse.HoverColor = System.Drawing.Color.Empty;
            this.linkLabelBrowse.Location = new System.Drawing.Point(190, 84);
            this.linkLabelBrowse.Name = "linkLabelBrowse";
            this.linkLabelBrowse.RegularColor = System.Drawing.Color.Empty;
            this.linkLabelBrowse.Size = new System.Drawing.Size(52, 14);
            this.linkLabelBrowse.TabIndex = 2;
            this.linkLabelBrowse.Text = "Browse...";
            this.linkLabelBrowse.Click += new System.EventHandler(this.linkLabelBrowse_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label4.Location = new System.Drawing.Point(190, 64);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(203, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Drag and drop an image onto this window";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(347, 178);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(266, 178);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "&Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Poster:";
            // 
            // pictureBoxPoster
            // 
            this.pictureBoxPoster.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxPoster.Image = global::BDHeroGUI.Properties.Resources.no_poster_w185;
            this.pictureBoxPoster.Location = new System.Drawing.Point(92, 64);
            this.pictureBoxPoster.Name = "pictureBoxPoster";
            this.pictureBoxPoster.Size = new System.Drawing.Size(92, 137);
            this.pictureBoxPoster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPoster.TabIndex = 4;
            this.pictureBoxPoster.TabStop = false;
            this.toolTipPictureBox.SetToolTip(this.pictureBoxPoster, "Browse computer for images...");
            this.pictureBoxPoster.Click += new System.EventHandler(this.pictureBoxPoster_Click);
            // 
            // textBoxYear
            // 
            this.textBoxYear.Location = new System.Drawing.Point(92, 38);
            this.textBoxYear.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.textBoxYear.Minimum = new decimal(new int[] {
            1800,
            0,
            0,
            0});
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.Size = new System.Drawing.Size(49, 20);
            this.textBoxYear.TabIndex = 1;
            this.textBoxYear.Value = new decimal(new int[] {
            1999,
            0,
            0,
            0});
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTitle.Location = new System.Drawing.Point(92, 12);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(330, 20);
            this.textBoxTitle.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Release Year:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Movie Title:";
            // 
            // FormMediaCustom
            // 
            this.AcceptButton = this.buttonSave;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(434, 213);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(9999, 252);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 252);
            this.Name = "FormMediaCustom";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Custom Metadata";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMediaCustom_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMediaCustom_DragEnter);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPoster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBoxYear)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.NumericUpDown textBoxYear;
        private System.Windows.Forms.PictureBox pictureBoxPoster;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label label3;
        private DotNetUtils.Controls.LinkLabel2 linkLabelURL;
        private DotNetUtils.Controls.LinkLabel2 linkLabelBrowse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolTip toolTipPictureBox;

    }
}