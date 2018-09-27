using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        }

        //Choose and save chosen directory
        private void directoryBtn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK && !String.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            {
                directoryPathLbl.Text = folderBrowserDialog.SelectedPath;
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
                gridObjects.Clear();

                //Loop through all file formats
                foreach (String format in fileFormats)
                {
                    //get all files of this format
                    String[] files = Directory.GetFiles(directoryPathLbl.Text, $"*.{format}", SearchOption.AllDirectories);

                    //if format is a gupta file, mark it as such
                    Boolean isApt = format == "apt";

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

            Match match = Regex.Match(filePath, "[^\\\\]*$");
            if (match.Success)
            {
                fileName = match.Value;
            }

            return fileName;
        }

        private void SearchForText(String filePath, Boolean isApt, String[] searchKeywords)
        {
            var lines = File.ReadAllLines(filePath);
            String fileName = GetFileNameFromPath(filePath);
            Int32 lineCounter = 0;

            //loop through lines looking for our text and filling in the table with found results
            foreach (String line in lines)
            {
                foreach (String keyword in searchKeywords)
                {
                    if (line.ToLower().Contains(keyword))
                    {
                        GridView view = new GridView()
                        {
                            SearchedText = keyword,
                            FileName = fileName,
                            RowNumber = lineCounter + 1
                        };

                        //if format of file is "apt" get class function/window name
                        if (isApt)
                        {
                            Boolean goBack = true;
                            Boolean classNameFound = false;
                            Int64 backwardsCounter = lineCounter - 1;

                            while (goBack && backwardsCounter > 0)
                            {
                                if (!classNameFound && (lines[backwardsCounter].Contains(".head 5 +  Function:") || lines[backwardsCounter].Contains(".head 3 +  Function:")))
                                {
                                    view.GuptaClassName = lines[backwardsCounter];
                                    classNameFound = true;
                                }

                                if (lines[backwardsCounter].Contains(".head 3 +  Functional Class:") || lines[backwardsCounter].Contains(".head 1 +  "))
                                {
                                    goBack = false;
                                    view.GuptaObjectName = lines[backwardsCounter];
                                }

                                backwardsCounter--;
                            }
                        }

                        //Depending on how many context lines we want to get for our search result
                        if (searchUpDown.Value > 1)
                        {
                            String resultLine = String.Empty;
                            Int32 resultLinesCounter = Convert.ToInt32(searchUpDown.Value);

                            //traverse backwards
                            for (Int32 i = resultLinesCounter; i > 0; i--)
                            {
                                if (lineCounter - i >= 0)
                                {
                                    resultLine += Environment.NewLine + lines[lineCounter - i];
                                }
                            }
                            //traverse forwards
                            for (Int32 i = 0; i <= resultLinesCounter; i++)
                            {
                                if (lineCounter + i < lines.Length)
                                {
                                    resultLine += Environment.NewLine + lines[lineCounter + i];
                                }
                            }

                            view.ResultText = resultLine;
                        }
                        else if (searchUpDown.Value == 1)
                            view.ResultText = line;

                        gridObjects.Add(view);
                    }
                }

                lineCounter++;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            formatsTxt.Size = new Size(this.Size.Width - 400, formatsTxt.Size.Height);
            searchTxt.Size = new Size(this.Size.Width - 240, searchTxt.Size.Height);
            dataGrid.Size = new Size(this.Size.Width - 43, this.Size.Height - 169);
        }

        private void LoadGrid()
        {
            dataGrid.Rows.Clear();

            foreach (GridView obj in gridObjects.OrderBy(obj => obj.SearchedText))
            {
                dataGrid.Rows.Add(obj.SearchedText, obj.FileName, obj.ResultText, obj.RowNumber, obj.GuptaObjectName, obj.GuptaClassName);
            }
        }
    }
}
