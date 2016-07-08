using System;

namespace SortingService.BusinessLayer.SortingAlgorithms
{
    /// <summary>
    /// A naive implementation for early testing
    /// </summary>
    [Obsolete]
    internal class NaiveSorting
    {
        /// <summary>
        /// Merges both arrays and sorts them
        /// </summary>
        /// <param name="newData"></param>
        /// <param name="alreadySortedData"></param>
        /// <returns></returns>
        internal string[] Sort(string[] newData, string[] alreadySortedData)
        {
            string[] mergedData = new string[alreadySortedData.Length + newData.Length];

            Array.Copy(alreadySortedData, mergedData, alreadySortedData.Length);
            Array.Copy(newData, 0, mergedData, alreadySortedData.Length, newData.Length);
            Array.Sort(mergedData);

            return mergedData;
        }
    }
}