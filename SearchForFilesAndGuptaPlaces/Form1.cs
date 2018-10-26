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

namespace SearchForFilesAndGuptaPlaces
{
    public partial class Form1 : Form
    {
        private List<GridView> gridObjects;

        public Form1()
        {
            InitializeComponent();

            gridObjects = new List<GridView>();

            if (File.Exists("Resources/if_search_b_44994.ico"))
                Icon = new Icon("Resources/if_search_b_44994.ico");

            //Display program version in the title
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            Text = $"{Text} {version.Major}.{version.Minor}";
        }

        private Int32 CurrentId;
        private const String notepadName = "notepad.exe";
        private const String notepadPPName = "Notepad++.exe";
        private const String notepadPPRegistryPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Notepad++";
        private const String guptaFunctionFunction = ".head 5 +  Function:";
        private const String guptaTableFunction = ".head 3 +  Function:";
        private const String guptaFunctionClass = ".head 3 +  Functional Class:";
        private const String guptaTableClass = ".head 1 +  ";
        private const String aptFormat = "apt";
        private const String fileNameRegex = "[^\\\\]*$";

        //Choose and save chosen directory
        private void directoryBtn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FolderSelectDialog folderSelectorDialog = new FolderSelectDialog() { Title = "Select a folder", InitialDirectory = @"c:\" };
            if (folderSelectorDialog.ShowDialog(IntPtr.Zero))
            {
                directoryPathLbl.Text = folderSelectorDialog.FileName;
            }
            Cursor.Current = Cursors.Arrow;
        }

        //Search for input text across all files with chosen formats inside our chosen directory
        private void searchBtn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (!String.IsNullOrWhiteSpace(directoryPathLbl.Text))
            {
                String[] fileFormats = formatsTxt.Text.Split(',');
                String[] searchKeywords = searchTxt.Text.Split(',');
                Boolean isApt;

                CurrentId = 1;
                gridObjects.Clear();

                //Loop through all file formats
                foreach (String format in fileFormats)
                {
                    //get all files of this format
                    String[] files = Directory.GetFiles(directoryPathLbl.Text, $"*.{format}", SearchOption.AllDirectories);

                    //if format is a gupta file, mark it as such
                    isApt = format == aptFormat;

                    //loop through all files searching for our text
                    foreach (String file in files)
                    {
                        SearchForText(file, isApt, searchKeywords);
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                LoadGrid();
            }
            Cursor.Current = Cursors.Arrow;
        }

        private String GetFileNameFromPath(String filePath)
        {
            String fileName = String.Empty;

            Match match = Regex.Match(filePath, fileNameRegex);
            if (match.Success)
            {
                fileName = match.Value;
            }

            return fileName;
        }

        //Search for text in a file
        private void SearchForText(String filePath, Boolean isApt, String[] searchKeywords)
        {
            if (searchKeywords.Any(key => !String.IsNullOrWhiteSpace(key)))
            {
                var lines = File.ReadAllLines(filePath);
                String fileName = GetFileNameFromPath(filePath);
                Int32 lineCounter = 0;

                //loop through lines looking for our text and filling in the table with found results
                foreach (String line in lines)
                {
                    foreach (String keyword in searchKeywords.Where(key => !String.IsNullOrWhiteSpace(key)))
                    {
                        if (line.ToLower().Contains(keyword))
                        {
                            GridView view = new GridView()
                            {
                                SearchedText = keyword,
                                FileName = fileName,
                                RowNumber = lineCounter + 1,
                                FilePath = filePath,
                                Id = CurrentId,
                                IsGuptaFile = isApt
                            };

                            CurrentId++;

                            //if format of file is "apt" get class function/window name
                            if (isApt)
                            {
                                Boolean goBack = true;
                                Boolean classNameFound = false;
                                Int64 backwardsCounter = lineCounter;

                                while (goBack && backwardsCounter > 0)
                                {
                                    if (!classNameFound && (lines[backwardsCounter].Contains(guptaFunctionFunction) || lines[backwardsCounter].Contains(guptaTableFunction)))
                                    {
                                        view.GuptaClassName = lines[backwardsCounter];
                                        classNameFound = true;
                                    }

                                    if (lines[backwardsCounter].Contains(guptaFunctionClass) || lines[backwardsCounter].Contains(guptaTableClass))
                                    {
                                        goBack = false;
                                        view.GuptaObjectName = lines[backwardsCounter];
                                    }

                                    backwardsCounter--;
                                }
                            }

                            gridObjects.Add(view);
                        }
                    }

                    lineCounter++;
                }
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //formatsTxt.Size = new Size((Int32)(this.Size.Width * 0.67), formatsTxt.Size.Height);
            //searchTxt.Size = new Size((Int32)(this.Size.Width * 0.8), searchTxt.Size.Height);
            dataGrid.Size = new Size((Int32)(this.Size.Width * 0.638), this.Size.Height - dataGrid.Location.Y - 50);
            previewTextBox.Size = new Size(this.Size.Width - dataGrid.Location.X - dataGrid.Size.Width - 30, dataGrid.Size.Height);
            previewTextBox.Location = new Point(dataGrid.Location.X + dataGrid.Size.Width + 6, previewTextBox.Location.Y);
            previewLbl.Location = new Point(previewTextBox.Location.X, previewLbl.Location.Y);
            
        }

        //Load grid from gridview list
        private void LoadGrid()
        {
            dataGrid.Rows.Clear();

            foreach (GridView obj in gridObjects
                .OrderBy(obj => obj.SearchedText)
                .ThenByDescending(obj => obj.IsGuptaFile)
                .ThenBy(obj => obj.FilePath)
                .ThenBy(obj => obj.RowNumber))
            {
                dataGrid.Rows.Add(obj.SearchedText, obj.FileName, obj.RowNumber, obj.GuptaObjectName, obj.GuptaClassName, obj.Id);
            }
        }

        //Opens a file in notepad++ if its installed, or notepad if not
        private void OpenTextFile(String filePath, Int32 lineToGoTo)
        {
            var nppDir = (String)Registry.GetValue(notepadPPRegistryPath, null, null);

            //if Notepad++ is installed
            if (!String.IsNullOrWhiteSpace(nppDir))
            {
                var nppExePath = Path.Combine(nppDir, notepadPPName);
                var sb = new StringBuilder();
                sb.AppendFormat("\"{0}\" -n{1}", filePath, lineToGoTo);
                Process.Start(nppExePath, sb.ToString());
            }
            else
            {
                Process.Start(notepadName, filePath);
            }
        }

        //Open the file after choosing the open button
        private void openFileBtn_Click(object sender, EventArgs e)
        {
            if (dataGrid.SelectedRows.Count > 0)
            {
                GridView selected = gridObjects.FirstOrDefault(g => g.Id == GetSelectedRowID());

                OpenTextFile(selected.FilePath, selected.RowNumber);
            }
        }

        //Open the file after double clicking
        private void dataGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            GridView selected = gridObjects.FirstOrDefault(g => g.Id == GetSelectedRowID());

            OpenTextFile(selected.FilePath, selected.RowNumber);
        }

        private void dataGrid_SelectionChanged(object sender, EventArgs e)
        {
            ReloadPreviewTextBox();
        }

        //Reloads preview text box if rules required for it to load are correct
        private void ReloadPreviewTextBox()
        {
            //If a row has been selected and we want to see a result line
            if (dataGrid.SelectedRows.Count > 0 && searchUpDown.Value > 0)
                previewTextBox.Text = GetResultPreview(gridObjects.FirstOrDefault(g => g.Id == GetSelectedRowID()));
            else if (searchUpDown.Value == 0)
                previewTextBox.Text = String.Empty;
        }

        private Int32 GetSelectedRowID()
        {
            return (Int32)dataGrid.SelectedRows[0].Cells["Id"].Value;
        }


        //Loads text lines from result object for preview
        private String GetResultPreview(GridView item)
        {
            String result = String.Empty;
            Int32 linesToDisplay = (Int32) searchUpDown.Value;

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

        //Reload preview for the selected row, since we changed how many rows we want to see
        private void searchUpDown_ValueChanged(object sender, EventArgs e)
        {
            ReloadPreviewTextBox();
        }
    }
}
