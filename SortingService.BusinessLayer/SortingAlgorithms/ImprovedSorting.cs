using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace SortingService.BusinessLayer.SortingAlgorithms
{
    class ImprovedSorting
    {
        public string Sort(string[] recievedData, string currentDataFile)
        {
            Array.Sort(recievedData);
            //TODO change the file path dynamically
            string chunkFilePath = Path.ChangeExtension(currentDataFile, ".srt1");
            File.WriteAllLines(chunkFilePath, recievedData);
            string mergedFilePath = Path.ChangeExtension(currentDataFile, ".srt1");

            //MemoryMappedFile is also an option, but in this case we prefer StreamReader because we just need to read the file line by line, not manipulate its memory
            using (var mergedFileStream = File.CreateText(mergedFilePath))
            using (StreamReader originalDataReader = new StreamReader(currentDataFile))
            using (StreamReader receivedDataReader = new StreamReader(chunkFilePath))
            {
                String originalDataLine = originalDataReader.ReadLine();
                String receivedDataLine = receivedDataReader.ReadLine();

                while (originalDataLine != null && receivedDataLine != null)
                {
                    if (originalDataLine == null)
                    {
                        mergedFileStream.WriteLine(receivedDataLine);
                        receivedDataLine = receivedDataReader.ReadLine();
                    }
                    else if (receivedDataLine == null || originalDataLine.CompareTo(receivedDataLine) < 0)
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
        }
    }
}
