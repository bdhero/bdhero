namespace LicenseUtils.Controls
{
    partial class AuthorLabel
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.labelYearRange = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.labelYearRange);
            this.flowLayoutPanel1.Controls.Add(this.labelName);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(431, 21);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "©";
            // 
            // labelYearRange
            // 
            this.labelYearRange.AutoSize = true;
            this.labelYearRange.Location = new System.Drawing.Point(25, 3);
            this.labelYearRange.Margin = new System.Windows.Forms.Padding(3);
            this.labelYearRange.Name = "labelYearRange";
            this.labelYearRange.Size = new System.Drawing.Size(58, 13);
            this.labelYearRange.TabIndex = 2;
            this.labelYearRange.Text = "2001-2005";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(89, 3);
            this.labelName.Margin = new System.Windows.Forms.Padding(3);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(69, 13);
            this.labelName.TabIndex = 3;
            this.labelName.Text = "Author Name";
            // 
            // AuthorLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "AuthorLabel";
            this.Size = new System.Drawing.Size(431, 21);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelYearRange;
        private System.Windows.Forms.Label labelName;
    }
}
