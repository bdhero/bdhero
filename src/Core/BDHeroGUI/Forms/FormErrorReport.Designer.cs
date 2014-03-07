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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.checkBoxDisabled = new System.Windows.Forms.CheckBox();
            this.textEditorControl1 = new TextEditor.WinForms.TextEditorControl();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(13, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(764, 549);
            this.panel1.TabIndex = 4;
            // 
            // checkBoxMultiline
            // 
            this.checkBoxMultiline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxMultiline.AutoSize = true;
            this.checkBoxMultiline.Checked = true;
            this.checkBoxMultiline.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMultiline.Location = new System.Drawing.Point(12, 619);
            this.checkBoxMultiline.Name = "checkBoxMultiline";
            this.checkBoxMultiline.Size = new System.Drawing.Size(64, 17);
            this.checkBoxMultiline.TabIndex = 5;
            this.checkBoxMultiline.Text = "&Multiline";
            this.checkBoxMultiline.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowLineNumbers
            // 
            this.checkBoxShowLineNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowLineNumbers.AutoSize = true;
            this.checkBoxShowLineNumbers.Checked = true;
            this.checkBoxShowLineNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowLineNumbers.Location = new System.Drawing.Point(82, 619);
            this.checkBoxShowLineNumbers.Name = "checkBoxShowLineNumbers";
            this.checkBoxShowLineNumbers.Size = new System.Drawing.Size(121, 17);
            this.checkBoxShowLineNumbers.TabIndex = 6;
            this.checkBoxShowLineNumbers.Text = "Show &Line Numbers";
            this.checkBoxShowLineNumbers.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowRuler
            // 
            this.checkBoxShowRuler.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowRuler.AutoSize = true;
            this.checkBoxShowRuler.Checked = true;
            this.checkBoxShowRuler.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowRuler.Location = new System.Drawing.Point(210, 619);
            this.checkBoxShowRuler.Name = "checkBoxShowRuler";
            this.checkBoxShowRuler.Size = new System.Drawing.Size(81, 17);
            this.checkBoxShowRuler.TabIndex = 7;
            this.checkBoxShowRuler.Text = "Show &Ruler";
            this.checkBoxShowRuler.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowWhitespace
            // 
            this.checkBoxShowWhitespace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowWhitespace.AutoSize = true;
            this.checkBoxShowWhitespace.Checked = true;
            this.checkBoxShowWhitespace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowWhitespace.Location = new System.Drawing.Point(298, 619);
            this.checkBoxShowWhitespace.Name = "checkBoxShowWhitespace";
            this.checkBoxShowWhitespace.Size = new System.Drawing.Size(113, 17);
            this.checkBoxShowWhitespace.TabIndex = 8;
            this.checkBoxShowWhitespace.Text = "Show &Whitespace";
            this.checkBoxShowWhitespace.UseVisualStyleBackColor = true;
            // 
            // checkBoxReadonly
            // 
            this.checkBoxReadonly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxReadonly.AutoSize = true;
            this.checkBoxReadonly.Location = new System.Drawing.Point(417, 619);
            this.checkBoxReadonly.Name = "checkBoxReadonly";
            this.checkBoxReadonly.Size = new System.Drawing.Size(71, 17);
            this.checkBoxReadonly.TabIndex = 9;
            this.checkBoxReadonly.Text = "Readonly";
            this.checkBoxReadonly.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(110, 9);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(667, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "TextBox:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "TextEditorControl:";
            // 
            // buttonAccept
            // 
            this.buttonAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAccept.Location = new System.Drawing.Point(702, 615);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(75, 23);
            this.buttonAccept.TabIndex = 11;
            this.buttonAccept.Text = "Accept: 0";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // checkBoxDisabled
            // 
            this.checkBoxDisabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxDisabled.AutoSize = true;
            this.checkBoxDisabled.Location = new System.Drawing.Point(495, 619);
            this.checkBoxDisabled.Name = "checkBoxDisabled";
            this.checkBoxDisabled.Size = new System.Drawing.Size(67, 17);
            this.checkBoxDisabled.TabIndex = 10;
            this.checkBoxDisabled.Text = "&Disabled";
            this.checkBoxDisabled.UseVisualStyleBackColor = true;
            // 
            // textEditorControl1
            // 
            this.textEditorControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textEditorControl1.ConvertTabsToSpaces = false;
            this.textEditorControl1.CutCopyWholeLine = false;
            this.textEditorControl1.Location = new System.Drawing.Point(110, 36);
            this.textEditorControl1.Multiline = false;
            this.textEditorControl1.Name = "textEditorControl1";
            this.textEditorControl1.Padding = new System.Windows.Forms.Padding(1);
            this.textEditorControl1.ShowColumnRuler = false;
            this.textEditorControl1.ShowLineNumbers = false;
            this.textEditorControl1.ShowWhiteSpace = false;
            this.textEditorControl1.Size = new System.Drawing.Size(667, 20);
            this.textEditorControl1.TabIndex = 3;
            this.textEditorControl1.Text = "C:\\Users\\%UserName%\\BDHero\\`~1!2@3#4$56^7&8*9(0)-_=+[{]}\\|;:\'\",<.>/?";
            // 
            // FormErrorReport
            // 
            this.AcceptButton = this.buttonAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 650);
            this.Controls.Add(this.checkBoxDisabled);
            this.Controls.Add(this.buttonAccept);
            this.Controls.Add(this.textEditorControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
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
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private TextEditor.WinForms.TextEditorControl textEditorControl1;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.CheckBox checkBoxDisabled;


    }
}