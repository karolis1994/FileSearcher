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

namespace SearchForFilesAndGuptaPlaces
{
    public enum FileType { apt, sql, other }

    public partial class Form1 : Form
    {
        private readonly List<GridView> gridObjects;

        public Form1()
        {
            InitializeComponent();

            gridObjects = new List<GridView>();

            if (File.Exists("Resources/if_search_b_44994.ico"))
                Icon = new Icon("Resources/if_search_b_44994.ico");

            //Display program version in the title
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = $"{Text} {version.Major}.{version.Minor}";

            formatsTxt.Text = defaultFormats;
            this.ActiveControl = searchTxt;
        }

        private Int32 CurrentId;
        private const String notepadName = "notepad.exe";
        private const String notepadPPName = "Notepad++.exe";
        private const String notepadPPRegistryPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Notepad++";
        private const String aptFormat = "apt";
        private const String sqlFormat = "sql";
        private const String fileNameRegex = "[^\\\\]*$";
        private const String defaultFormats = "sql,apt";
        private readonly String[] guptaFunctions = new String[2] { ".head 5 +  Function:", ".head 3 +  Function:" };
        private readonly String[] guptaClasses = new String[2] { ".head 3 +  Functional Class:", ".head 1 +  " };
        private readonly String[] sqlHeaders = new String[4] { "PROCEDURE", "FUNCTION", "PACKAGE", "TRIGGER" };

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
        private void SearchBtn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (!String.IsNullOrWhiteSpace(directoryPathLbl.Text))
            {
                String[] fileFormats = formatsTxt.Text.Split(',');
                String[] searchKeywords;
                if (separatorTextBox.Text.Length == 1)
                    searchKeywords = searchTxt.Text.Split(separatorTextBox.Text[0]);
                else
                    searchKeywords = new String[] { searchTxt.Text };

                List<FileView> files = new List<FileView>();

                CurrentId = 1;
                gridObjects.Clear();

                //Loop through all file formats
                foreach (String format in fileFormats)
                {
                    //determine format
                    FileType fileType = FileType.other;
                    if (format == aptFormat)
                        fileType = FileType.apt;
                    else if (format == sqlFormat)
                        fileType = FileType.sql;

                    //get all files of this format
                    foreach (String filePath in Directory.GetFiles(directoryPathLbl.Text, $"*.{format}", SearchOption.AllDirectories))
                        files.Add(new FileView() { FilePath = filePath, FileType = fileType });
                }

                Parallel.ForEach(files, i => FindTextInFile(i, searchKeywords));

                GC.Collect();
                GC.WaitForPendingFinalizers();

                LoadGrid();
            }
            Cursor.Current = Cursors.Arrow;
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

            foreach (GridView obj in gridObjects
                .OrderBy(obj => obj.SearchedText)
                .ThenByDescending(obj => obj.IsGuptaFile)
                .ThenBy(obj => obj.FilePath)
                .ThenBy(obj => obj.RowNumber))
            {
                dataGrid.Rows.Add(obj.SearchedText, obj.FileName, obj.RowNumber, obj.ObjectName, obj.ClassName, obj.Id);
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
                GridView selected = gridObjects.FirstOrDefault(g => g.Id == GetSelectedRowID());

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
            GridView selected = gridObjects.FirstOrDefault(g => g.Id == GetSelectedRowID());

            OpenTextFile(selected.FilePath, selected.RowNumber);
        }
        /// <summary>
        /// On grid row selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_SelectionChanged(object sender, EventArgs e)
        {
            ReloadPreviewTextBox();
        }
        /// <summary>
        /// Reload preview for the selected row, since we changed how many rows we want to see
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchUpDown_ValueChanged(object sender, EventArgs e)
        {
            ReloadPreviewTextBox();
        }

        /// <summary>
        /// Search the file for keywords and load all of the results into gridObjects list
        /// </summary>
        /// <param name="file"></param>
        /// <param name="searchKeywords"></param>
        private void FindTextInFile(FileView file, String[] searchKeywords)
        {
            List<GridView> localResults = new List<GridView>();

            if (searchKeywords.Any(key => !String.IsNullOrWhiteSpace(key)))
            {
                var lines = File.ReadAllLines(file.FilePath);
                String fileName = GetFileNameFromPath(file.FilePath);
                Int32 lineCounter = 0;

                //loop through lines looking for our text and filling in the table with found results
                foreach (String line in lines.Select(s => s.ToUpperInvariant()))
                {
                    foreach (String keyword in searchKeywords.Where(key => !String.IsNullOrWhiteSpace(key)))
                    {
                        if (line.Contains(keyword))
                        {
                            GridView view = new GridView()
                            {
                                SearchedText = keyword,
                                FileName = fileName,
                                RowNumber = lineCounter + 1,
                                FilePath = file.FilePath,
                                Id = CurrentId,
                                IsGuptaFile = file.FileType == FileType.apt
                            };

                            CurrentId++;

                            //if format of file is "apt" get class function/window name
                            if (file.FileType == FileType.apt || file.FileType == FileType.sql)
                            {
                                Boolean goBack = true;
                                Boolean classNameFound = false;
                                Int64 backwardsCounter = lineCounter;

                                if (file.FileType == FileType.apt)
                                    while (goBack && backwardsCounter > 0)
                                    {
                                        if (!classNameFound && (guptaFunctions.Any(lines[backwardsCounter].Contains)))
                                        {
                                            view.ClassName = ParseObjectName(lines[backwardsCounter], guptaFunctions);
                                            classNameFound = true;
                                        }

                                        if (guptaClasses.Any(lines[backwardsCounter].Contains))
                                        {
                                            goBack = false;
                                            view.ObjectName = ParseObjectName(lines[backwardsCounter], guptaClasses);
                                        }

                                        backwardsCounter--;
                                    }
                                else
                                    while (goBack && backwardsCounter >= 0)
                                    {
                                        if (sqlHeaders.Any(lines[backwardsCounter].ToUpperInvariant().Contains))
                                        {
                                            goBack = false;
                                            view.ClassName = ParseObjectName(lines[backwardsCounter], sqlHeaders, true);
                                        }

                                        backwardsCounter--;
                                    }
                            }

                            localResults.Add(view);
                        }
                    }

                    lineCounter++;
                }

                //Add results to the final list
                if (localResults.Count > 0)
                    lock (gridObjects)
                    {
                        gridObjects.AddRange(localResults);
                    }
            }
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
        private void ReloadPreviewTextBox()
        {
            //If a row has been selected and we want to see a result line
            if (dataGrid.SelectedRows.Count > 0 && searchUpDown.Value > 0)
                previewTextBox.Text = GetResultPreview(gridObjects.FirstOrDefault(g => g.Id == GetSelectedRowID()));
            else if (searchUpDown.Value == 0)
                previewTextBox.Text = String.Empty;
        }
        /// <summary>
        /// Parse file path and return 
        /// </summary>
        /// <param name="filePath">Path to the file</param>
        /// <returns>File name</returns>
        private static String GetFileNameFromPath(String filePath)
        {
            String fileName = String.Empty;

            Match match = Regex.Match(filePath, fileNameRegex);
            if (match.Success)
            {
                fileName = match.Value;
            }

            return fileName;
        }
        /// <summary>
        /// Parses line of text and shortens it taking away the keywords
        /// </summary>
        /// <param name="line">Line of text</param>
        /// <param name="keywords">Keywords matching function/method/procedure... names</param>
        /// <param name="caseInsensitive">If true, turns the line to uppercase when comparing</param>
        /// <returns></returns>
        private static String ParseObjectName(String line, String[] keywords, Boolean caseInsensitive = false)
        {
            String tempLine = caseInsensitive ? line.ToUpperInvariant() : line;

            foreach(var k in keywords)
            {
                if (tempLine.Contains(k))
                {
                    String result = line.Substring(tempLine.IndexOf(k, StringComparison.InvariantCulture) + k.Length);
                    Int32 index = result.IndexOf('(');
                    if (index != -1)
                        return result.Substring(0, index);

                    return result;
                }
            }

            return line;
        }
        /// <summary>
        /// Loads text lines from result object for preview
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private String GetResultPreview(GridView item)
        {
            String result = String.Empty;
            Int32 linesToDisplay = (Int32)searchUpDown.Value;

            if (item != null)
            {
                //When we want to see a single line, we skip all lines until result line
                Int32 startingLine = item.RowNumber - 1;

                //if we want to see more find out how much is half of our lines and set resultRowNumber - halfOfLines to be skipped.
                //This is done to keep the search result around the middle of preview window
                if (linesToDisplay > 1)
                {
                    Decimal temp = searchUpDown.Value / 2;
                    Int32 halfCounter = (Int32)Math.Round(temp, 0, MidpointRounding.AwayFromZero);

                    startingLine = item.RowNumber - halfCounter;

                    //If our result row number - half of preview lines is a negative number, simply set it back to 0
                    if (startingLine < 0)
                        startingLine = 0;
                }

                //Join the selected lines into a single string
                result = String.Join(Environment.NewLine, File.ReadLines(item.FilePath).Skip(startingLine).Take(linesToDisplay));
            }

            return result;
        }
        /// <summary>
        /// Returns the ID of first selected row object
        /// </summary>
        /// <returns>Id of selected row object</returns>
        private Int32 GetSelectedRowID()
        {
            return (Int32)dataGrid.SelectedRows[0].Cells["Id"].Value;
        }


    }
}
