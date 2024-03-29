﻿namespace SearchForFilesAndGuptaPlaces
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
            this.SearchedText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FoundInRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GuptaObject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.directoryBtn = new System.Windows.Forms.Button();
            this.directoryLbl = new System.Windows.Forms.Label();
            this.directoryPathLbl = new System.Windows.Forms.Label();
            this.searchBtn = new System.Windows.Forms.Button();
            this.searchLbl = new System.Windows.Forms.Label();
            this.searchTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.searchUpDown = new System.Windows.Forms.NumericUpDown();
            this.openFileBtn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.previewTextBox = new System.Windows.Forms.TextBox();
            this.previewLbl = new System.Windows.Forms.Label();
            this.separatorTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // formatsTxt
            // 
            this.formatsTxt.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.formatsTxt.Location = new System.Drawing.Point(372, 38);
            this.formatsTxt.Name = "formatsTxt";
            this.formatsTxt.Size = new System.Drawing.Size(830, 20);
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
            this.FoundInRow,
            this.GuptaObject,
            this.ClassName,
            this.Id});
            this.dataGrid.Location = new System.Drawing.Point(15, 139);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            this.dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid.Size = new System.Drawing.Size(785, 297);
            this.dataGrid.TabIndex = 2;
            this.dataGrid.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGrid_CellContentDoubleClick);
            this.dataGrid.SelectionChanged += new System.EventHandler(this.DataGrid_SelectionChanged);
            // 
            // SearchedText
            // 
            this.SearchedText.HeaderText = "Searched tex";
            this.SearchedText.Name = "SearchedText";
            this.SearchedText.ReadOnly = true;
            this.SearchedText.Width = 200;
            // 
            // FileName
            // 
            this.FileName.HeaderText = "File name";
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.FileName.Width = 300;
            // 
            // FoundInRow
            // 
            this.FoundInRow.HeaderText = "Row where the text was found";
            this.FoundInRow.Name = "FoundInRow";
            this.FoundInRow.ReadOnly = true;
            this.FoundInRow.Width = 200;
            // 
            // GuptaObject
            // 
            this.GuptaObject.HeaderText = "Gupta object";
            this.GuptaObject.Name = "GuptaObject";
            this.GuptaObject.ReadOnly = true;
            this.GuptaObject.Width = 300;
            // 
            // ClassName
            // 
            this.ClassName.HeaderText = "Function name";
            this.ClassName.Name = "ClassName";
            this.ClassName.ReadOnly = true;
            this.ClassName.Width = 300;
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // directoryBtn
            // 
            this.directoryBtn.Location = new System.Drawing.Point(15, 57);
            this.directoryBtn.Name = "directoryBtn";
            this.directoryBtn.Size = new System.Drawing.Size(239, 23);
            this.directoryBtn.TabIndex = 3;
            this.directoryBtn.Text = "Choose directory to browse";
            this.directoryBtn.UseVisualStyleBackColor = true;
            this.directoryBtn.Click += new System.EventHandler(this.DirectoryBtn_Click);
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
            this.searchBtn.Click += new System.EventHandler(this.SearchBtn_Click);
            // 
            // searchLbl
            // 
            this.searchLbl.AutoSize = true;
            this.searchLbl.Location = new System.Drawing.Point(12, 15);
            this.searchLbl.Name = "searchLbl";
            this.searchLbl.Size = new System.Drawing.Size(93, 13);
            this.searchLbl.TabIndex = 8;
            this.searchLbl.Text = "Text to search for:";
            // 
            // searchTxt
            // 
            this.searchTxt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.searchTxt.Location = new System.Drawing.Point(111, 12);
            this.searchTxt.Name = "searchTxt";
            this.searchTxt.Size = new System.Drawing.Size(977, 20);
            this.searchTxt.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(260, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Number of preview lines to display:";
            // 
            // searchUpDown
            // 
            this.searchUpDown.Location = new System.Drawing.Point(436, 86);
            this.searchUpDown.Name = "searchUpDown";
            this.searchUpDown.Size = new System.Drawing.Size(120, 20);
            this.searchUpDown.TabIndex = 11;
            this.searchUpDown.ValueChanged += new System.EventHandler(this.SearchUpDown_ValueChanged);
            // 
            // openFileBtn
            // 
            this.openFileBtn.Location = new System.Drawing.Point(15, 115);
            this.openFileBtn.Name = "openFileBtn";
            this.openFileBtn.Size = new System.Drawing.Size(239, 23);
            this.openFileBtn.TabIndex = 12;
            this.openFileBtn.Text = "Open first selected file";
            this.openFileBtn.UseVisualStyleBackColor = true;
            this.openFileBtn.Click += new System.EventHandler(this.OpenFileBtn_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // previewTextBox
            // 
            this.previewTextBox.Location = new System.Drawing.Point(806, 141);
            this.previewTextBox.Multiline = true;
            this.previewTextBox.Name = "previewTextBox";
            this.previewTextBox.ReadOnly = true;
            this.previewTextBox.Size = new System.Drawing.Size(396, 297);
            this.previewTextBox.TabIndex = 13;
            // 
            // previewLbl
            // 
            this.previewLbl.AutoSize = true;
            this.previewLbl.Location = new System.Drawing.Point(806, 125);
            this.previewLbl.Name = "previewLbl";
            this.previewLbl.Size = new System.Drawing.Size(119, 13);
            this.previewLbl.TabIndex = 14;
            this.previewLbl.Text = "Preview selected result:";
            // 
            // separatorTextBox
            // 
            this.separatorTextBox.Location = new System.Drawing.Point(1156, 12);
            this.separatorTextBox.MaxLength = 1;
            this.separatorTextBox.Name = "separatorTextBox";
            this.separatorTextBox.Size = new System.Drawing.Size(42, 20);
            this.separatorTextBox.TabIndex = 15;
            this.separatorTextBox.Text = ",";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1094, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Separator:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1214, 448);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.separatorTextBox);
            this.Controls.Add(this.previewLbl);
            this.Controls.Add(this.previewTextBox);
            this.Controls.Add(this.openFileBtn);
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
            this.Text = "Text searcher";
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
        private System.Windows.Forms.Button openFileBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox previewTextBox;
        private System.Windows.Forms.Label previewLbl;
        private System.Windows.Forms.DataGridViewTextBoxColumn SearchedText;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FoundInRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn GuptaObject;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.TextBox separatorTextBox;
        private System.Windows.Forms.Label label2;
    }
}

