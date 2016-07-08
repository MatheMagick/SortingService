using System;
using System.IO;
using SortingService.Client16GBFile.ServiceReference1;

namespace SortingService.Client16GBFile
{
    internal class SortingServiceWrapper
    {
        internal void SortFile(string filePath, string sortedFilePath)
        {
            using (var client = new SortingServiceClient())
            {
                var sessionId = client.BeginStream();
                SendFile(client, sessionId, filePath);

                using (var result = client.GetSortedStream(sessionId))
                using (var fileStream = File.Create("sortedLargeFile.txt"))
                {
                    result.CopyTo(fileStream);
                }

                client.EndStream(sessionId);
            }
        }

        private void SendFile(SortingServiceClient client, Guid sessionId, string filePath)
        {
            int linesInAChunk = Settings.Default.LinesPerChunk;

            using (var largeFileStream = File.OpenRead(filePath))
            using (var textStream = new StreamReader(largeFileStream))
            {
                bool endOfFile = false;
                string[] chunk = new string[linesInAChunk];
                string textFileRow;

                while (!endOfFile)
                {
                    for (int i = 0; i < linesInAChunk; i++)
                    {
                        if (( textFileRow = textStream.ReadLine() ) == null)
                        {
                            endOfFile = true;
                            // Resize the array so we remove the unneeded elements
                            Array.Resize(ref chunk, i + 1);
                            break;
                        }

                        chunk[i] = textFileRow;
                    }

                    client.PutStreamData(sessionId, chunk);
                }
            }
        }
    }
}