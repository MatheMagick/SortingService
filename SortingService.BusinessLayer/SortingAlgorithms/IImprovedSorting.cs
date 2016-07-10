namespace SortingService.BusinessLayer.SortingAlgorithms
{
    public interface IImprovedSorting
    {

        /// <summary>
        /// Merges two sorted files into one
        /// </summary>
        /// <param name="currentSortedDataFile">First sorted file path</param>
        /// <param name="chunkFilePath">Second sorted file path</param>
        string MergeSortedFiles(string currentSortedDataFile, string chunkFilePath);

        /// <summary>
        /// Sorts the data provided and merges it with the already sorted file.
        /// </summary>
        /// <param name="recievedData">A buffer with the received data that is not sorted</param>
        /// <param name="currentSortedDataFile">The path to the file that contains the already sorted data to merge</param>
        /// <returns></returns>
        string Sort(string[] recievedData, string currentSortedDataFile);
    }
}