using System;

namespace SortingService.Client16GBFile
{
    /// <summary>
    /// A test client for sending and sorting a file that is several gigabytes long via the <see cref="ServiceReferenceSortingService.ISortingService"/> service
    /// </summary>
    class Program
    {
        static void Main()
        {
            string filePath = CreateLargeFile();

            new SortingServiceWrapper().SortFile(filePath, Settings.Default.SortedFilePath);

            Console.WriteLine($"Large file sorted. The resulting file is \"{Settings.Default.SortedFilePath}\"");

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static string CreateLargeFile()
        {
            string largeFilePath = Settings.Default.LargeFilePath;
            LargeFileGenerator fileGenerator = new LargeFileGenerator();
            byte fileSizeInGB = Settings.Default.FileSizeInGB;
            int charactersPerFileLine = Settings.Default.CharactersPerFileLine;

            fileGenerator.CreateLargeFile(largeFilePath, fileSizeInGB, charactersPerFileLine);

            return largeFilePath;
        }
    }
}