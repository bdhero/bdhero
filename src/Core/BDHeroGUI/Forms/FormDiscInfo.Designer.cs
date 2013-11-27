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
            this.label1 = new System.Windows.Forms.Label();
            this.labelQuickSummary = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.discInfoMetadataPanel = new BDHeroGUI.Components.DiscInfoMetadataPanel();
            this.discInfoFeaturesPanel = new BDHeroGUI.Components.DiscInfoFeaturesPanel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "BD-ROM:";
            // 
            // labelQuickSummary
            // 
            this.labelQuickSummary.AutoSize = true;
            this.labelQuickSummary.Location = new System.Drawing.Point(73, 13);
            this.labelQuickSummary.Name = "labelQuickSummary";
            this.labelQuickSummary.Size = new System.Drawing.Size(110, 13);
            this.labelQuickSummary.TabIndex = 1;
            this.labelQuickSummary.Text = "VOLUME_LABEL D:\\";
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(1030, 444);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // discInfoMetadataPanel
            // 
            this.discInfoMetadataPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.discInfoMetadataPanel.Location = new System.Drawing.Point(152, 44);
            this.discInfoMetadataPanel.Name = "discInfoMetadataPanel";
            this.discInfoMetadataPanel.Size = new System.Drawing.Size(953, 394);
            this.discInfoMetadataPanel.TabIndex = 0;
            // 
            // discInfoFeaturesPanel
            // 
            this.discInfoFeaturesPanel.Location = new System.Drawing.Point(12, 44);
            this.discInfoFeaturesPanel.Name = "discInfoFeaturesPanel";
            this.discInfoFeaturesPanel.Size = new System.Drawing.Size(134, 182);
            this.discInfoFeaturesPanel.TabIndex = 0;
            // 
            // FormDiscInfo
            // 
            this.AcceptButton = this.buttonClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(1117, 479);
            this.Controls.Add(this.discInfoMetadataPanel);
            this.Controls.Add(this.discInfoFeaturesPanel);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.labelQuickSummary);
            this.Controls.Add(this.label1);
            this.Name = "FormDiscInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Disc Info";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelQuickSummary;
        private Components.DiscInfoMetadataPanel discInfoMetadataPanel;
        private System.Windows.Forms.Button buttonClose;
        private Components.DiscInfoFeaturesPanel discInfoFeaturesPanel;
    }
}