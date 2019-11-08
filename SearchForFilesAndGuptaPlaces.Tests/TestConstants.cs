using SearchForFilesAndGuptaPlaces.Models;
using System.IO;

namespace SearchForFilesAndGuptaPlaces.Tests
{
    public static class TestConstants
    {
        public const string SearchKeywordTest = "TEST";
        public const string SearchKeywordGibberish = "dsafdfgdfghdf";
        public const string SearchKeywordSix = "six";
        public const string FilesDirectoryPath = "./Resources";
        public const string FileFormatSql = "sql";
        public const string FileFormatTxt = "txt";

        public const int SearchKeywordSixFoundAtRowNumber = 6;
        public const int OutOfBoundsRowNumber = 9999;

        public static SearchResult FoundResult => new SearchResult()
        {
            RowNumber = SearchKeywordSixFoundAtRowNumber,
            FilePath = Directory.GetCurrentDirectory() + "/Resources/TestSearch.txt",
            FileName = "TestSearch.txt",
            Id = "RandomId",
            SearchKeyword = SearchKeywordSix,
            IsGuptaFile = false
        };
        public static SearchResult NotFoundResult => new SearchResult()
        {
            RowNumber = OutOfBoundsRowNumber,
            FilePath = Directory.GetCurrentDirectory() + "/Resources/TestSearch.txt",
            FileName = "TestSearch.txt",
            Id = "RandomId",
            SearchKeyword = SearchKeywordGibberish,
            IsGuptaFile = false
        };
    }
}
