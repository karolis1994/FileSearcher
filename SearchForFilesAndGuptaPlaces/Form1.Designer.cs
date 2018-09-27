namespace SearchForFilesAndGuptaPlaces
{
    partial class Form1
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
            this.formatsTxt = new System.Windows.Forms.TextBox();
            this.formatsLbl = new System.Windows.Forms.Label();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.directoryBtn = new System.Windows.Forms.Button();
            this.directoryLbl = new System.Windows.Forms.Label();
            this.directoryPathLbl = new System.Windows.Forms.Label();
            this.searchBtn = new System.Windows.Forms.Button();
            this.searchLbl = new System.Windows.Forms.Label();
            this.searchTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.searchUpDown = new System.Windows.Forms.NumericUpDown();
            this.SearchedText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TextRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FoundInRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GuptaObject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // formatsTxt
            // 
            this.formatsTxt.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.formatsTxt.Location = new System.Drawing.Point(372, 38);
            this.formatsTxt.Name = "formatsTxt";
            this.formatsTxt.Size = new System.Drawing.Size(416, 20);
            this.formatsTxt.TabIndex = 0;
            // 
            // formatsLbl
            // 
            this.formatsLbl.AutoSize = true;
            this.formatsLbl.Location = new System.Drawing.Point(12, 41);
            this.formatsLbl.Name = "formatsLbl";
            this.formatsLbl.Size = new System.Drawing.Size(354, 13);
            this.formatsLbl.TabIndex = 1;
            this.formatsLbl.Text = "Input file formats to browse through separated by comma (e.g. apt, sql, txt)";
            // 
            // dataGrid
            // 
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToDeleteRows = false;
            this.dataGrid.AllowUserToOrderColumns = true;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SearchedText,
            this.FileName,
            this.TextRow,
            this.FoundInRow,
            this.GuptaObject});
            this.dataGrid.Location = new System.Drawing.Point(15, 119);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.Size = new System.Drawing.Size(773, 319);
            this.dataGrid.TabIndex = 2;
            // 
            // directoryBtn
            // 
            this.directoryBtn.Location = new System.Drawing.Point(15, 57);
            this.directoryBtn.Name = "directoryBtn";
            this.directoryBtn.Size = new System.Drawing.Size(239, 23);
            this.directoryBtn.TabIndex = 3;
            this.directoryBtn.Text = "Choose directory to browse";
            this.directoryBtn.UseVisualStyleBackColor = true;
            this.directoryBtn.Click += new System.EventHandler(this.directoryBtn_Click);
            // 
            // directoryLbl
            // 
            this.directoryLbl.AutoSize = true;
            this.directoryLbl.Location = new System.Drawing.Point(260, 61);
            this.directoryLbl.Name = "directoryLbl";
            this.directoryLbl.Size = new System.Drawing.Size(76, 13);
            this.directoryLbl.TabIndex = 4;
            this.directoryLbl.Text = "Directory path:";
            // 
            // directoryPathLbl
            // 
            this.directoryPathLbl.AutoSize = true;
            this.directoryPathLbl.Location = new System.Drawing.Point(342, 62);
            this.directoryPathLbl.Name = "directoryPathLbl";
            this.directoryPathLbl.Size = new System.Drawing.Size(0, 13);
            this.directoryPathLbl.TabIndex = 5;
            // 
            // searchBtn
            // 
            this.searchBtn.Location = new System.Drawing.Point(15, 86);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(239, 23);
            this.searchBtn.TabIndex = 6;
            this.searchBtn.Text = "Search";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // searchLbl
            // 
            this.searchLbl.AutoSize = true;
            this.searchLbl.Location = new System.Drawing.Point(12, 15);
            this.searchLbl.Name = "searchLbl";
            this.searchLbl.Size = new System.Drawing.Size(194, 13);
            this.searchLbl.TabIndex = 8;
            this.searchLbl.Text = "Text to search for separated by comma:";
            // 
            // searchTxt
            // 
            this.searchTxt.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.searchTxt.Location = new System.Drawing.Point(212, 12);
            this.searchTxt.Name = "searchTxt";
            this.searchTxt.Size = new System.Drawing.Size(576, 20);
            this.searchTxt.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(260, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Number of context lines to add to result";
            // 
            // searchUpDown
            // 
            this.searchUpDown.Location = new System.Drawing.Point(457, 86);
            this.searchUpDown.Name = "searchUpDown";
            this.searchUpDown.Size = new System.Drawing.Size(120, 20);
            this.searchUpDown.TabIndex = 11;
            // 
            // SearchedText
            // 
            this.SearchedText.HeaderText = "Searched tex";
            this.SearchedText.Name = "SearchedText";
            // 
            // FileName
            // 
            this.FileName.HeaderText = "File name";
            this.FileName.Name = "FileName";
            this.FileName.Width = 200;
            // 
            // TextRow
            // 
            this.TextRow.HeaderText = "Text row";
            this.TextRow.Name = "TextRow";
            // 
            // FoundInRow
            // 
            this.FoundInRow.HeaderText = "Row where the text was found";
            this.FoundInRow.Name = "FoundInRow";
            this.FoundInRow.Width = 200;
            // 
            // GuptaObject
            // 
            this.GuptaObject.HeaderText = "Gupta object";
            this.GuptaObject.Name = "GuptaObject";
            this.GuptaObject.Width = 200;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.searchUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.searchLbl);
            this.Controls.Add(this.searchTxt);
            this.Controls.Add(this.searchBtn);
            this.Controls.Add(this.directoryPathLbl);
            this.Controls.Add(this.directoryLbl);
            this.Controls.Add(this.directoryBtn);
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(this.formatsLbl);
            this.Controls.Add(this.formatsTxt);
            this.Name = "Form1";
            this.Text = "Search for text in gupta and elsewhere";
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox formatsTxt;
        private System.Windows.Forms.Label formatsLbl;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button directoryBtn;
        private System.Windows.Forms.Label directoryLbl;
        private System.Windows.Forms.Label directoryPathLbl;
        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.Label searchLbl;
        private System.Windows.Forms.TextBox searchTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown searchUpDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn SearchedText;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TextRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn FoundInRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn GuptaObject;
    }
}

