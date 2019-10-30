using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchForFilesAndGuptaPlaces.Services;
using System.Linq;

namespace SearchForFilesAndGuptaPlaces.Tests
{
    [TestClass]
    public class FileSearchServiceTests
    {
        private static IFileSearchService GetFileSearchServiceImplementation()
        {
            return new FileSearchService();
        }

        [DataTestMethod]
        [DataRow(new string[] { TestConstants.FileFormatSql })]
        [DataRow(new string[] { TestConstants.FileFormatTxt })]
        [DataRow(new string[] { TestConstants.FileFormatSql, TestConstants.FileFormatTxt })]
        public async Task GetFiles_Success(string[] fileFormats)
        {
            //Arrange
            var service = GetFileSearchServiceImplementation();
            var testDirectory = Path.GetFullPath(TestConstants.FilesDirectoryPath);

            //Act
            var files = await service.GetFilesAsync(fileFormats, testDirectory);

            //Assert
            Assert.IsTrue(files.Count() > 0);
        }

        [DataTestMethod]
        [DataRow(new string[] { "apt" })]
        [DataRow(new string[] { "xxx" })]
        [DataRow(new string[] { "we", "testing", "this", "shizzle" })]
        public async Task GetFiles_Fail(string[] fileFormats)
        {
            //Arrange
            var service = GetFileSearchServiceImplementation();
            var testDirectory = Path.GetFullPath(TestConstants.FilesDirectoryPath);

            //Act
            var files = await service.GetFilesAsync(fileFormats, testDirectory);

            //Assert
            Assert.IsTrue(files.Count() == 0);
        }

        [DataTestMethod]
        [DataRow(TestConstants.FileFormatSql)]
        [DataRow(TestConstants.FileFormatTxt)]
        public async Task FindInFile_TestString_FoundInFile(string fileFormat)
        {
            //Arrange
            var service = GetFileSearchServiceImplementation();
            var testDirectory = Path.GetFullPath(TestConstants.FilesDirectoryPath);
            var files = (await service.GetFilesAsync(new string[] { fileFormat }, testDirectory)).ToArray();

            //Act
            var results = await service.FindInFileAsync(files[0], new string[] { TestConstants.SearchKeywordTest });

            //Assert
            Assert.IsTrue(results.Count() > 0);
        }

        [DataTestMethod]
        [DataRow(TestConstants.FileFormatSql)]
        [DataRow(TestConstants.FileFormatTxt)]
        public async Task FindInFile_TestString_NotFoundInFile(string fileFormat)
        {
            //Arrange
            var service = GetFileSearchServiceImplementation();
            var testDirectory = Path.GetFullPath(TestConstants.FilesDirectoryPath);
            var files = (await service.GetFilesAsync(new string[] { fileFormat }, testDirectory)).ToArray();

            //Act
            var results = await service.FindInFileAsync(files[0], new string[] { TestConstants.SearchKeywordGibberish });

            //Assert
            Assert.IsTrue(results.Count() == 0);
        }
    }
}
