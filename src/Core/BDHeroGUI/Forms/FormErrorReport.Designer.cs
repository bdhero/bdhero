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
            this.checkBoxShowLineNumbers = new System.Windows.Forms.CheckBox();
            this.checkBoxShowRuler = new System.Windows.Forms.CheckBox();
            this.checkBoxShowWhitespace = new System.Windows.Forms.CheckBox();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxWordWrap = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(82, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(695, 574);
            this.panel1.TabIndex = 3;
            // 
            // checkBoxShowLineNumbers
            // 
            this.checkBoxShowLineNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowLineNumbers.AutoSize = true;
            this.checkBoxShowLineNumbers.Location = new System.Drawing.Point(169, 619);
            this.checkBoxShowLineNumbers.Name = "checkBoxShowLineNumbers";
            this.checkBoxShowLineNumbers.Size = new System.Drawing.Size(121, 17);
            this.checkBoxShowLineNumbers.TabIndex = 5;
            this.checkBoxShowLineNumbers.Text = "Show &Line Numbers";
            this.checkBoxShowLineNumbers.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowRuler
            // 
            this.checkBoxShowRuler.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowRuler.AutoSize = true;
            this.checkBoxShowRuler.Checked = true;
            this.checkBoxShowRuler.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowRuler.Location = new System.Drawing.Point(82, 619);
            this.checkBoxShowRuler.Name = "checkBoxShowRuler";
            this.checkBoxShowRuler.Size = new System.Drawing.Size(81, 17);
            this.checkBoxShowRuler.TabIndex = 4;
            this.checkBoxShowRuler.Text = "Show &Ruler";
            this.checkBoxShowRuler.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowWhitespace
            // 
            this.checkBoxShowWhitespace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowWhitespace.AutoSize = true;
            this.checkBoxShowWhitespace.Location = new System.Drawing.Point(296, 619);
            this.checkBoxShowWhitespace.Name = "checkBoxShowWhitespace";
            this.checkBoxShowWhitespace.Size = new System.Drawing.Size(113, 17);
            this.checkBoxShowWhitespace.TabIndex = 6;
            this.checkBoxShowWhitespace.Text = "Show &Whitespace";
            this.checkBoxShowWhitespace.UseVisualStyleBackColor = true;
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTitle.Location = new System.Drawing.Point(82, 9);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(695, 20);
            this.textBoxTitle.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Issue Title:";
            // 
            // buttonAccept
            // 
            this.buttonAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAccept.Location = new System.Drawing.Point(621, 615);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(75, 23);
            this.buttonAccept.TabIndex = 8;
            this.buttonAccept.Text = "&Submit";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(702, 615);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Description:";
            // 
            // checkBoxWordWrap
            // 
            this.checkBoxWordWrap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxWordWrap.AutoSize = true;
            this.checkBoxWordWrap.Location = new System.Drawing.Point(415, 619);
            this.checkBoxWordWrap.Name = "checkBoxWordWrap";
            this.checkBoxWordWrap.Size = new System.Drawing.Size(78, 17);
            this.checkBoxWordWrap.TabIndex = 7;
            this.checkBoxWordWrap.Text = "Word &wrap";
            this.checkBoxWordWrap.UseVisualStyleBackColor = true;
            // 
            // FormErrorReport
            // 
            this.AcceptButton = this.buttonAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(789, 650);
            this.Controls.Add(this.checkBoxWordWrap);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAccept);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.checkBoxShowWhitespace);
            this.Controls.Add(this.checkBoxShowRuler);
            this.Controls.Add(this.checkBoxShowLineNumbers);
            this.Controls.Add(this.panel1);
            this.Name = "FormErrorReport";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBoxShowLineNumbers;
        private System.Windows.Forms.CheckBox checkBoxShowRuler;
        private System.Windows.Forms.CheckBox checkBoxShowWhitespace;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBoxWordWrap;


    }
}