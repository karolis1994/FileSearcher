using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchForFilesAndGuptaPlaces.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SearchForFilesAndGuptaPlaces.Tests
{
    [TestClass]
    public class FileSearchServiceTests
    {

        [DataTestMethod]
        [DataRow(new string[] { TestConstants.FileFormatSql })]
        [DataRow(new string[] { TestConstants.FileFormatTxt })]
        [DataRow(new string[] { TestConstants.FileFormatSql, TestConstants.FileFormatTxt })]
        public async Task GetFiles_Success(string[] fileFormats)
        {
            //Arrange
            var service = TestInterfaceImplementations.GetFileSearchServiceImplementation();
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
            var service = TestInterfaceImplementations.GetFileSearchServiceImplementation();
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
            var service = TestInterfaceImplementations.GetFileSearchServiceImplementation();
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
            var service = TestInterfaceImplementations.GetFileSearchServiceImplementation();
            var testDirectory = Path.GetFullPath(TestConstants.FilesDirectoryPath);
            var files = (await service.GetFilesAsync(new string[] { fileFormat }, testDirectory)).ToArray();

            //Act
            var results = await service.FindInFileAsync(files[0], new string[] { TestConstants.SearchKeywordGibberish });

            //Assert
            Assert.IsTrue(results.Count() == 0);
        }

        [TestMethod]
        public async Task GetResultPreviewAsync_ResultExists()
        {
            //Arrange
            SearchResult search = TestConstants.FoundResult;
            var service = TestInterfaceImplementations.GetFileSearchServiceImplementation();

            //Act
            var result = await service.GetResultPreviewAsync(search, 1);

            //Assert
            Assert.IsTrue(!String.IsNullOrWhiteSpace(result));
        }

        [TestMethod]
        public async Task GetResultPreviewAsync_ResultDoesntExist()
        {
            //Arrange
            SearchResult search = TestConstants.NotFoundResult;
            var service = TestInterfaceImplementations.GetFileSearchServiceImplementation();

            //Act
            var result = await service.GetResultPreviewAsync(search, 1);

            //Assert
            Assert.IsTrue(String.IsNullOrWhiteSpace(result));
        }
    }
}
