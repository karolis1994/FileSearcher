using SearchForFilesAndGuptaPlaces.Services;
using System;
using System.Windows.Forms;

namespace SearchForFilesAndGuptaPlaces
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IFileSearchService fileSearchService = new FileSearchService();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(fileSearchService));
        }
    }
}
