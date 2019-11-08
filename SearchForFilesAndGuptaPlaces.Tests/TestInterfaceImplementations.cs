using SearchForFilesAndGuptaPlaces.Services;

namespace SearchForFilesAndGuptaPlaces.Tests
{
    public static class TestInterfaceImplementations
    {
        public static IFileSearchService GetFileSearchServiceImplementation()
        {
            return new FileSearchService();
        }
    }
}
