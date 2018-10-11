using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            Icon = new Icon("Resources/if_search_b_44994.ico");
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

                            //Depending on how many context lines we want to get for our search result, collect neccessary text lines
                            if (searchUpDown.Value > 1)
                            {
                                String resultLine = String.Empty;
                                StringBuilder sb = new StringBuilder();
                                Int32 resultLinesCounter = Convert.ToInt32(searchUpDown.Value);

                                //traverse backwards
                                for (Int32 i = resultLinesCounter; i > 0; i--)
                                {
                                    if (lineCounter - i >= 0)
                                    {
                                        sb.Append(Environment.NewLine + lines[lineCounter - i]);
                                    }
                                }
                                //traverse forwards
                                for (Int32 i = 0; i <= resultLinesCounter; i++)
                                {
                                    if (lineCounter + i < lines.Length)
                                    {
                                        sb.Append(Environment.NewLine + lines[lineCounter + i]);
                                    }
                                }

                                view.ResultText = sb.ToString();
                            }
                            else if (searchUpDown.Value == 1)
                                view.ResultText = line;

                            gridObjects.Add(view);
                        }
                    }

                    lineCounter++;
                }
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            formatsTxt.Size = new Size(this.Size.Width - 400, formatsTxt.Size.Height);
            searchTxt.Size = new Size(this.Size.Width - 240, searchTxt.Size.Height);
            dataGrid.Size = new Size(this.Size.Width - 43, this.Size.Height - 191);
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
                dataGrid.Rows.Add(obj.SearchedText, obj.FileName, obj.ResultText, obj.RowNumber, obj.GuptaObjectName, obj.GuptaClassName, obj.Id);
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

        private void openFileBtn_Click(object sender, EventArgs e)
        {
            if (dataGrid.SelectedRows.Count > 0)
            {
                Int32 id = (Int32)dataGrid.SelectedRows[0].Cells["Id"].Value;
                GridView selected = gridObjects.FirstOrDefault(g => g.Id == id);

                OpenTextFile(selected.FilePath, selected.RowNumber);
            }
        }

        private void dataGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            GridView selected = gridObjects.FirstOrDefault(g => g.Id == (Int32) dataGrid.Rows[e.RowIndex].Cells["Id"].Value);

            OpenTextFile(selected.FilePath, selected.RowNumber);
        }
    }
}
