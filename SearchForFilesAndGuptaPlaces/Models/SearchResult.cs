using System;

namespace SearchForFilesAndGuptaPlaces.Models
{
    /// <summary>
    /// Search result model
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gupta object name
        /// </summary>
        public string ObjectName { get; set; }
        /// <summary>
        /// Gupta object class name
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// Text according to which the result was found
        /// </summary>
        public string SearchKeyword { get; set; }
        /// <summary>
        /// File name where the result was found
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// File path of the file where the result was found
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Is the file a gupta file
        /// </summary>
        public bool IsGuptaFile { get; set; }
        /// <summary>
        /// Row number where the result was found in the file
        /// </summary>
        public int RowNumber { get; set; }
        /// <summary>
        /// Search result identificator
        /// </summary>
        public string Id { get; set; }
    }
}
