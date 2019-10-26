using SearchForFilesAndGuptaPlaces.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchForFilesAndGuptaPlaces.Services
{
    public interface IFileSearchService
    {
        /// <summary>
        /// Search the file for keywords and returns a collection of search results
        /// </summary>
        /// <param name="file"></param>
        /// <param name="searchKeywords"></param>
        Task<IEnumerable<SearchResult>> FindInFileAsync(FileInfo file, String[] searchKeywords);
        /// <summary>
        /// Loads text lines from result object for preview
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<String> GetResultPreview(SearchResult item, int linesToDisplay);
        /// <summary>
        /// Retrieves a list of files and their information inside a directory and its subsequent directiories
        /// </summary>
        /// <param name="startingDirectory"></param>
        /// <param name="fileFormats"></param>
        /// <returns></returns>
        Task<IEnumerable<FileInfo>> GetFiles(string[] fileFormats, string startingDirectory);
    }
}
