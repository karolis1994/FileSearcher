using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Globalization;
using SearchForFilesAndGuptaPlaces.Models;
using Easy.Common;
using SearchForFilesAndGuptaPlaces.Services;
using System.Threading;

namespace SearchForFilesAndGuptaPlaces
{
    public partial class Form1 : Form
    {
        private object _lock = new object();
        private readonly List<SearchResult> gridObjects;
        private readonly IFileSearchService fileSearchService;

        public Form1(IFileSearchService fileSearchService)
        {
            InitializeComponent();

            this.fileSearchService = fileSearchService;
            gridObjects = new List<SearchResult>();

            if (File.Exists("Resources/if_search_b_44994.ico"))
                Icon = new Icon("Resources/if_search_b_44994.ico");

            //Display program version in the title
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = $"{Text} {version.Major}.{version.Minor}";

            formatsTxt.Text = defaultFormats;
            this.ActiveControl = searchTxt;
        }

        private const String notepadName = "notepad.exe";
        private const String notepadPPName = "Notepad++.exe";
        private const String notepadPPRegistryPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Notepad++";
        private const String defaultFormats = "sql,apt";

        /// <summary>
        /// Choose and save chosen directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirectoryBtn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FolderSelectDialog folderSelectorDialog = new FolderSelectDialog() { Title = "Select a folder", InitialDirectory = @"c:\" };
            if (folderSelectorDialog.ShowDialog(IntPtr.Zero))
            {
                directoryPathLbl.Text = folderSelectorDialog.FileName;
            }
            Cursor.Current = Cursors.Arrow;
        }
        /// <summary>
        /// Search for input text across all files with chosen formats inside our chosen directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchBtn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            searchBtn.Enabled = false;
            gridObjects.Clear();

            //Get files
            var files = await fileSearchService.GetFiles(formatsTxt.Text.Split(','), directoryPathLbl.Text).ConfigureAwait(false);
            //Go through files in parallel searching them
            var searchResultArrays = await Task.WhenAll(files.Select(f => fileSearchService.FindInFileAsync(f, searchTxt.Text.Split(separatorTextBox.Text[0]))).ToArray())
                .ConfigureAwait(false);
            var searchResults = searchResultArrays.SelectMany(f => f);

            gridObjects.AddRange(searchResults);

            //For some reason threads switch here and i get exception for cross-threading....
            this.Invoke((Action)(() => { 
                LoadGrid();
                searchBtn.Enabled = true;
                Cursor.Current = Cursors.Arrow;
            }));
        }
        /// <summary>
        /// Resize result boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //formatsTxt.Size = new Size((Int32)(this.Size.Width * 0.67), formatsTxt.Size.Height);
            //searchTxt.Size = new Size((Int32)(this.Size.Width * 0.8), searchTxt.Size.Height);
            dataGrid.Size = new Size((Int32)(this.Size.Width * 0.638), this.Size.Height - dataGrid.Location.Y - 50);
            previewTextBox.Size = new Size(this.Size.Width - dataGrid.Location.X - dataGrid.Size.Width - 30, dataGrid.Size.Height);
            previewTextBox.Location = new Point(dataGrid.Location.X + dataGrid.Size.Width + 6, previewTextBox.Location.Y);
            previewLbl.Location = new Point(previewTextBox.Location.X, previewLbl.Location.Y);
            
        }
        /// <summary>
        /// Load grid from gridview list
        /// </summary>
        private void LoadGrid()
        {
            dataGrid.Rows.Clear();

            foreach (SearchResult obj in gridObjects
                .OrderBy(obj => obj.SearchKeyword)
                .ThenByDescending(obj => obj.IsGuptaFile)
                .ThenBy(obj => obj.FilePath)
                .ThenBy(obj => obj.RowNumber))
            {
                dataGrid.Rows.Add(obj.SearchKeyword, obj.FileName, obj.RowNumber, obj.ObjectName, obj.ClassName, obj.Id);
            }
        }
        /// <summary>
        /// Open the file after choosing the open button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFileBtn_Click(object sender, EventArgs e)
        {
            if (dataGrid.SelectedRows.Count > 0)
            {
                SearchResult selected = gridObjects.FirstOrDefault(g => g.Id == GetSelectedRowID());

                OpenTextFile(selected.FilePath, selected.RowNumber);
            }
        }
        /// <summary>
        /// Open the file after double clicking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SearchResult selected = gridObjects.FirstOrDefault(g => g.Id == GetSelectedRowID());

            OpenTextFile(selected.FilePath, selected.RowNumber);
        }
        /// <summary>
        /// On grid row selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DataGrid_SelectionChanged(object sender, EventArgs e)
        {
            await ReloadPreviewTextBox().ConfigureAwait(false);
        }
        /// <summary>
        /// Reload preview for the selected row, since we changed how many rows we want to see
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchUpDown_ValueChanged(object sender, EventArgs e)
        {
            await ReloadPreviewTextBox().ConfigureAwait(false);
        }
        /// <summary>
        /// Opens a file in notepad++ if its installed, or notepad if not
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="lineToGoTo"></param>
        private void OpenTextFile(String filePath, Int32 lineToGoTo)
        {
            var nppDir = (String)Registry.GetValue(notepadPPRegistryPath, null, null);

            //if Notepad++ is installed
            if (!String.IsNullOrWhiteSpace(nppDir))
            {
                var nppExePath = Path.Combine(nppDir, notepadPPName);
                var sb = new StringBuilder();
                sb.AppendFormat(CultureInfo.InvariantCulture, "\"{0}\" -n{1}", filePath, lineToGoTo);
                Process.Start(nppExePath, sb.ToString());
            }
            else
            {
                Process.Start(notepadName, filePath);
            }
        }
        /// <summary>
        /// Reloads preview text box if rules required for it to load are correct
        /// </summary>
        private async Task ReloadPreviewTextBox()
        {
            var result = String.Empty;

            //If a row has been selected and we want to see a result line
            if (dataGrid.SelectedRows.Count > 0 && searchUpDown.Value > 0)
                result = await fileSearchService
                    .GetResultPreview(gridObjects.FirstOrDefault(g => g.Id == GetSelectedRowID()), (int)searchUpDown.Value)
                    .ConfigureAwait(false);

            this.Invoke((Action)(() => previewTextBox.Text = result));
        }
        /// <summary>
        /// Returns the ID of first selected row object
        /// </summary>
        /// <returns>Id of selected row object</returns>
        private string GetSelectedRowID()
        {
            return (string)dataGrid.SelectedRows[0].Cells["Id"].Value;
        }


    }
}
