namespace BDHeroGUI.Forms
{
    partial class FormErrorReport
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxMultiline = new System.Windows.Forms.CheckBox();
            this.checkBoxShowLineNumbers = new System.Windows.Forms.CheckBox();
            this.checkBoxShowRuler = new System.Windows.Forms.CheckBox();
            this.checkBoxShowWhitespace = new System.Windows.Forms.CheckBox();
            this.checkBoxReadonly = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(764, 602);
            this.panel1.TabIndex = 0;
            // 
            // checkBoxMultiline
            // 
            this.checkBoxMultiline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxMultiline.AutoSize = true;
            this.checkBoxMultiline.Checked = true;
            this.checkBoxMultiline.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMultiline.Location = new System.Drawing.Point(13, 621);
            this.checkBoxMultiline.Name = "checkBoxMultiline";
            this.checkBoxMultiline.Size = new System.Drawing.Size(64, 17);
            this.checkBoxMultiline.TabIndex = 1;
            this.checkBoxMultiline.Text = "&Multiline";
            this.checkBoxMultiline.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowLineNumbers
            // 
            this.checkBoxShowLineNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowLineNumbers.AutoSize = true;
            this.checkBoxShowLineNumbers.Checked = true;
            this.checkBoxShowLineNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowLineNumbers.Location = new System.Drawing.Point(83, 621);
            this.checkBoxShowLineNumbers.Name = "checkBoxShowLineNumbers";
            this.checkBoxShowLineNumbers.Size = new System.Drawing.Size(121, 17);
            this.checkBoxShowLineNumbers.TabIndex = 2;
            this.checkBoxShowLineNumbers.Text = "Show &Line Numbers";
            this.checkBoxShowLineNumbers.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowRuler
            // 
            this.checkBoxShowRuler.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowRuler.AutoSize = true;
            this.checkBoxShowRuler.Checked = true;
            this.checkBoxShowRuler.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowRuler.Location = new System.Drawing.Point(211, 621);
            this.checkBoxShowRuler.Name = "checkBoxShowRuler";
            this.checkBoxShowRuler.Size = new System.Drawing.Size(81, 17);
            this.checkBoxShowRuler.TabIndex = 3;
            this.checkBoxShowRuler.Text = "Show &Ruler";
            this.checkBoxShowRuler.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowWhitespace
            // 
            this.checkBoxShowWhitespace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowWhitespace.AutoSize = true;
            this.checkBoxShowWhitespace.Checked = true;
            this.checkBoxShowWhitespace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowWhitespace.Location = new System.Drawing.Point(299, 621);
            this.checkBoxShowWhitespace.Name = "checkBoxShowWhitespace";
            this.checkBoxShowWhitespace.Size = new System.Drawing.Size(113, 17);
            this.checkBoxShowWhitespace.TabIndex = 4;
            this.checkBoxShowWhitespace.Text = "Show &Whitespace";
            this.checkBoxShowWhitespace.UseVisualStyleBackColor = true;
            // 
            // checkBoxReadonly
            // 
            this.checkBoxReadonly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxReadonly.AutoSize = true;
            this.checkBoxReadonly.Location = new System.Drawing.Point(697, 621);
            this.checkBoxReadonly.Name = "checkBoxReadonly";
            this.checkBoxReadonly.Size = new System.Drawing.Size(71, 17);
            this.checkBoxReadonly.TabIndex = 5;
            this.checkBoxReadonly.Text = "Readonly";
            this.checkBoxReadonly.UseVisualStyleBackColor = true;
            // 
            // FormErrorReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 650);
            this.Controls.Add(this.checkBoxReadonly);
            this.Controls.Add(this.checkBoxShowWhitespace);
            this.Controls.Add(this.checkBoxShowRuler);
            this.Controls.Add(this.checkBoxShowLineNumbers);
            this.Controls.Add(this.checkBoxMultiline);
            this.Controls.Add(this.panel1);
            this.Name = "FormErrorReport";
            this.Text = "FormErrorReport";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBoxMultiline;
        private System.Windows.Forms.CheckBox checkBoxShowLineNumbers;
        private System.Windows.Forms.CheckBox checkBoxShowRuler;
        private System.Windows.Forms.CheckBox checkBoxShowWhitespace;
        private System.Windows.Forms.CheckBox checkBoxReadonly;


    }
}