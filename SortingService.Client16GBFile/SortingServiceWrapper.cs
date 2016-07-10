using System;
using System.IO;
using SortingService.Client16GBFile.ServiceReference1;

namespace SortingService.Client16GBFile
{
    /// <summary>
    /// A wrapper around the <see cref="ISortingService"/> service
    /// </summary>
    internal class SortingServiceWrapper
    {
        internal void SortFile(string filePath, string sortedFilePath)
        {
            using (var client = new SortingServiceClient())
            {
                var sessionId = client.BeginStream();
                SendFile(client, sessionId, filePath);

                using (var result = client.GetSortedStream(sessionId))
                using (var fileStream = File.Create(sortedFilePath))
                {
                    result.CopyTo(fileStream);
                }

                client.EndStream(sessionId);
            }
        }

        private void SendFile(SortingServiceClient client, Guid sessionId, string filePath)
        {
            // Each chunk has a limit on maximum amount of lines to be sent
            int linesInAChunk = Settings.Default.LinesPerChunk;

            using (var largeFileStream = File.OpenRead(filePath))
            using (var textStream = new StreamReader(largeFileStream))
            {
                bool endOfFile = false;
                string[] chunk = new string[linesInAChunk];

                while (!endOfFile)
                {
                    for (int i = 0; i < linesInAChunk; i++)
                    {
                        string textFileRow;

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