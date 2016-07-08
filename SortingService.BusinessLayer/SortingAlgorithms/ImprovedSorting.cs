using System;
using System.IO;

namespace SortingService.BusinessLayer.SortingAlgorithms
{
    /// <summary>
    /// A class for sorting using a simplified version of merge sort
    /// </summary>
    internal class ImprovedSorting
    {
        /// <summary>
        /// Sorts the data provided and merges it with the already sorted file.
        /// </summary>
        /// <param name="recievedData">A buffer with the received data that is not sorted</param>
        /// <param name="currentSortedDataFile">The path to the file that contains the already sorted data to merge</param>
        /// <returns></returns>
        public string Sort(string[] recievedData, string currentSortedDataFile)
        {
            // TODO Another option is to use windows' integrated "sort" command.
            // It is optimized for sorting and has a switch for limiting memory usage, but in my tests my algorithm performs better
            // The reason being I know that the content so far is sorted, and the only sorting I need to do is on the receivedData only
            // Afterwards it's a simple merge of two files.

            // The default comparer is lexicographical, so it's OK to use it
            Array.Sort(recievedData);

            string chunkFilePath = Path.ChangeExtension(currentSortedDataFile, ".chunk");
            // Dump the received data to a file so in case of an outage we don't lose it and it gets processed upon startup
            File.WriteAllLines(chunkFilePath, recievedData);

            string mergedFilePath = Path.ChangeExtension(currentSortedDataFile, ".merged");

            //MemoryMappedFile is also an option, but in this case we prefer StreamReader
            //because we just need to read the file line by line, not manipulate its memory in-place
            using (var mergedFileStream = File.CreateText(mergedFilePath))
            using (var originalDataReader = new StreamReader(currentSortedDataFile))
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