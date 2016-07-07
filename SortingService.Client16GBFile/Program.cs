using System;
using System.IO;
using SortingService.Client16GBFile.ServiceReferenceSortingService;

namespace SortingService.Client16GBFile
{
    class Program
    {
        static void Main(string[] args)
        {
            LargeFileGenerator fileGenerator = new LargeFileGenerator();
            byte fileSizeInGB = Settings.Default.FileSizeInGB;
            var largeFilePath = fileGenerator.CreateLargeFile(1, 100);

            using (var client = new SortingServiceClient())
            {
                var sessionId = client.BeginStream();
                const int linesInAChunk = 1000000;

                using(var largeFileStream = File.OpenRead(largeFilePath))
                using(var textStream = new StreamReader(largeFileStream))
                {
                    bool endOfFile = false;
                    string[] chunk = new string[linesInAChunk];

                    while(!endOfFile)
                    {
                        for (int i = 0; i < linesInAChunk; i++)
                        {
                            var row = textStream.ReadLine();

                            if(row == null)
                            {
                                endOfFile = true;
                                var newArray = new string[i + 1];
                                Array.Copy(chunk, newArray, i + 1);
                                chunk = newArray;
                                break;
                            }

                            chunk[i] = row;
                        }

                        client.PutStreamData(sessionId, chunk);
                    }
                }

                using (var result = client.GetSortedStream(sessionId))
                using (var fileStream = File.Create("sortedLargeFile.txt"))
                {
                    result.CopyTo(fileStream);
                }

                client.EndStream(sessionId);

                //try
                //{
                //    SendChunk(client, sessionId);
                //}
                //catch (FaultException)
                //{
                //    // It is expected to get a FaultException in this case
                //    Console.WriteLine("Trying to access closed session results in expected behavior");
                //}
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
