using System;
using System.IO;

namespace SortingService.BusinessLayer.SortingAlgorithms
{
    class ImprovedSorting
    {
        public string Sort(string[] recievedData, string currentDataFile)
        {
            Array.Sort(recievedData);

            string chunkFilePath = Path.ChangeExtension(currentDataFile, ".chunk");
            // Dump the received data to a file so in case of an outage we don't lose it and it gets processed upon startup
            File.WriteAllLines(chunkFilePath, recievedData);

            string mergedFilePath = Path.ChangeExtension(currentDataFile, ".merged");

            //MemoryMappedFile is also an option, but in this case we prefer StreamReader
            //because we just need to read the file line by line, not manipulate its memory in-place
            using (var mergedFileStream = File.CreateText(mergedFilePath))
            using (var originalDataReader = new StreamReader(currentDataFile))
            using (var receivedDataReader = new StreamReader(chunkFilePath))
            {
                string originalDataLine = originalDataReader.ReadLine();
                string receivedDataLine = receivedDataReader.ReadLine();

                while (originalDataLine != null || receivedDataLine != null)
                {
                    if (originalDataLine == null)
                    {
                        mergedFileStream.WriteLine(receivedDataLine);
                        receivedDataLine = receivedDataReader.ReadLine();
                    }
                    else if (receivedDataLine == null || string.Compare(originalDataLine, receivedDataLine, StringComparison.OrdinalIgnoreCase) < 1)
                    {
                        mergedFileStream.WriteLine(originalDataLine);
                        originalDataLine = originalDataReader.ReadLine();
                    }
                    else
                    {
                        mergedFileStream.WriteLine(receivedDataLine);
                        receivedDataLine = receivedDataReader.ReadLine();
                    }
                }
            }

            File.Delete(chunkFilePath);

            return mergedFilePath;
        }
    }
}