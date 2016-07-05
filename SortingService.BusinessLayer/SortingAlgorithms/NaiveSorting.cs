using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingService.BusinessLayer.SortingAlgorithms
{
    internal class NaiveSorting
    {
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
