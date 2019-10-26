namespace SearchForFilesAndGuptaPlaces.Models
{
    /// <summary>
    /// File information model containing all the data relevant to the search
    /// </summary>
    public class FileInfo
    {
        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// File path
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// File type
        /// </summary>
        public FileType FileType { get; set; }
    }
}
