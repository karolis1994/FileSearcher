using Easy.Common;
using SearchForFilesAndGuptaPlaces.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SearchForFilesAndGuptaPlaces.Services
{
    public class FileSearchService : IFileSearchService
    {
        private const string _AptFormat = "apt";
        private const string _SqlFormat = "sql";
        private const string _FileNameRegex = "[^\\\\]*$";
        private readonly String[] guptaFunctions = new String[2] { ".head 5 +  Function:", ".head 3 +  Function:" };
        private readonly String[] guptaClasses = new String[2] { ".head 3 +  Functional Class:", ".head 1 +  " };
        private readonly String[] sqlHeaders = new String[4] { "PROCEDURE", "FUNCTION", "PACKAGE", "TRIGGER" };
        private object _lock = new object();

        public async Task<IEnumerable<SearchResult>> FindInFileAsync(Models.FileInfo file, string[] searchKeywords)
        {
            return await Task.Run(() =>
                {
                    List<SearchResult> localResults = new List<SearchResult>();

                    if (searchKeywords.Any(key => !String.IsNullOrWhiteSpace(key)))
                    {
                        var lines = File.ReadAllLines(file.FilePath);
                        Int32 lineCounter = 0;

                        //loop through lines looking for our text and filling in the table with found results
                        foreach (var line in lines.Select(s => s.ToUpperInvariant()))
                        {
                            foreach (String keyword in searchKeywords.Where(key => !String.IsNullOrWhiteSpace(key)))
                            {
                                if (line.Contains(keyword))
                                {
                                    SearchResult view = new SearchResult()
                                    {
                                        SearchKeyword = keyword,
                                        FileName = file.FileName,
                                        RowNumber = lineCounter + 1,
                                        FilePath = file.FilePath,
                                        Id = IDGenerator.Instance.Next,
                                        IsGuptaFile = file.FileType == FileType.Apt
                                    };

                                    //if format of file is "Apt" get class function/window name
                                    if (file.FileType == FileType.Apt || file.FileType == FileType.Sql)
                                    {
                                        Boolean goBack = true;
                                        Boolean classNameFound = false;
                                        Int64 backwardsCounter = lineCounter;

                                        if (file.FileType == FileType.Apt)
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

                                    lock (_lock)
                                        localResults.Add(view);
                                }
                            }

                            lock (_lock)
                                lineCounter++;
                        }
                    }

                    return localResults;
                })
                .ConfigureAwait(false);
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

            foreach (var k in keywords)
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
        /// Uses regex pattern to retrieve the file name from file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task<string> GetFileNameFromPath(String filePath)
        {
            return await Task.Run(() =>
                {
                    String fileName = String.Empty;

                    Match match = Regex.Match(filePath, _FileNameRegex);
                    if (match.Success)
                    {
                        fileName = match.Value;
                    }

                    return fileName;
                })
                .ConfigureAwait(false);
        }

        public async Task<String> GetResultPreview(SearchResult item, int linesToDisplay)
        {
            return await Task.Run(() =>
                {
                    String result = String.Empty;

                    if (item != null)
                    {
                        //When we want to see a single line, we skip all lines until result line
                        Int32 startingLine = item.RowNumber - 1;

                        //if we want to see more find out how much is half of our lines and set resultRowNumber - halfOfLines to be skipped.
                        //This is done to keep the search result around the middle of preview window
                        if (linesToDisplay > 1)
                        {
                            Decimal temp = linesToDisplay / 2;
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
                })
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Models.FileInfo>> GetFiles(string[] fileFormats, string startingDirectory)
        {
            if (!String.IsNullOrWhiteSpace(startingDirectory))
            {
                List<Models.FileInfo> files = new List<Models.FileInfo>();

                //Loop through all file formats
                foreach (String format in fileFormats)
                {
                    //determine format
                    FileType fileType;
                    if (format == _AptFormat)
                        fileType = FileType.Apt;
                    else if (format == _SqlFormat)
                        fileType = FileType.Sql;
                    else
                        fileType = FileType.Other;

                    //get all files of this format
                    foreach (String filePath in Directory.GetFiles(startingDirectory, $"*.{format}", SearchOption.AllDirectories))
                        files.Add(new Models.FileInfo()
                        {
                            FilePath = filePath,
                            FileType = fileType,
                            FileName = await GetFileNameFromPath(filePath).ConfigureAwait(false)
                        });
                }

                return files;

                //Parallel.ForEach(files, i => FindTextInFile(i, searchKeywords));
                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            }

            return Enumerable.Empty<Models.FileInfo>();
        }
    }
}
