namespace BDHeroGUI.Components
{
    partial class ChaptersPanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSearchResults = new System.Windows.Forms.ComboBox();
            this.listViewChapters = new DotNetUtils.Controls.ListView2();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chapters:";
            // 
            // comboBoxSearchResults
            // 
            this.comboBoxSearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSearchResults.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSearchResults.FormattingEnabled = true;
            this.comboBoxSearchResults.Location = new System.Drawing.Point(3, 16);
            this.comboBoxSearchResults.Name = "comboBoxSearchResults";
            this.comboBoxSearchResults.Size = new System.Drawing.Size(698, 21);
            this.comboBoxSearchResults.TabIndex = 1;
            // 
            // listViewChapters
            // 
            this.listViewChapters.AllowColumnReorder = true;
            this.listViewChapters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewChapters.CheckBoxes = true;
            this.listViewChapters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderTime,
            this.columnHeaderIndex});
            this.listViewChapters.FullRowSelect = true;
            this.listViewChapters.GridLines = true;
            this.listViewChapters.HideSelection = false;
            this.listViewChapters.LabelEdit = true;
            this.listViewChapters.Location = new System.Drawing.Point(3, 43);
            this.listViewChapters.MultiSelect = false;
            this.listViewChapters.Name = "listViewChapters";
            this.listViewChapters.ShowItemToolTips = true;
            this.listViewChapters.Size = new System.Drawing.Size(698, 429);
            this.listViewChapters.TabIndex = 2;
            this.listViewChapters.UseCompatibleStateImageBehavior = false;
            this.listViewChapters.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.DisplayIndex = 1;
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 200;
            // 
            // columnHeaderTime
            // 
            this.columnHeaderTime.DisplayIndex = 2;
            this.columnHeaderTime.Text = "Time";
            this.columnHeaderTime.Width = 80;
            // 
            // columnHeaderIndex
            // 
            this.columnHeaderIndex.DisplayIndex = 0;
            this.columnHeaderIndex.Text = "#";
            this.columnHeaderIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderIndex.Width = 40;
            // 
            // ChaptersPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewChapters);
            this.Controls.Add(this.comboBoxSearchResults);
            this.Controls.Add(this.label1);
            this.Name = "ChaptersPanel";
            this.Size = new System.Drawing.Size(704, 475);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSearchResults;
        private DotNetUtils.Controls.ListView2 listViewChapters;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderTime;
        private System.Windows.Forms.ColumnHeader columnHeaderIndex;
    }
}
