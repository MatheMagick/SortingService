using System;
using System.IO;
using SortingService.Clients.Common;

namespace SortingService.Client16GBFile
{
    /// <summary>
    /// Creates a large file, consisting of equally sized lines with random alphanumeric characters
    /// </summary>
    internal class LargeFileGenerator
    {
        // Since we tolerate errors in size we can use the 1000 multiplier instead of 1024
        private const int BytesInOneGigabyte = 1000 * 1000 * 1000;

        /// <summary>
        /// Creates a large file, consisting of equally sized lines with random alphanumeric characters
        /// </summary>
        /// <param name="fileName">The name of the file to be created</param>
        /// <param name="sizeInGB">Its size in GB</param>
        /// <param name="charactersPerLine">How many characters should be in each line</param>
        internal void CreateLargeFile(string fileName, byte sizeInGB, int charactersPerLine)
        {
            if(sizeInGB == 0)
            {
                throw new ArgumentException("Size in GB must be more than 0", nameof(sizeInGB));
            }

            if (charactersPerLine < 10 || charactersPerLine > 10000)
            {
                throw new ArgumentException("Characters per line  must be between than 10 and 10000", nameof(charactersPerLine));
            }

            long fileSizeInBytes = (long)sizeInGB * BytesInOneGigabyte; 

            if(!FileExistsAndHasCorrectSize(fileName, fileSizeInBytes))
            {
                CreateRandomTextFile(fileName, fileSizeInBytes, charactersPerLine);
            }
        }

        private bool FileExistsAndHasCorrectSize(string fileName, long fileSizeInBytes)
        {
            // Allow for file size error due to file system
            double sizeErrorTolerationInAbsolute = Settings.Default.LargeFileSizeErrorTolerationInPercents / 100;

            return File.Exists(fileName) &&
                   new FileInfo(fileName).Length > fileSizeInBytes * (1.0 - sizeErrorTolerationInAbsolute ) &&
                   new FileInfo(fileName).Length < fileSizeInBytes * (1.0 + sizeErrorTolerationInAbsolute );
        }

        private void CreateRandomTextFile(string fileName, long fileSizeInBytes, int charactersPerLine)
        {
            using (var fileStream = File.CreateText(fileName))
            {
                // Since we use alphanumeric characters only, each character is 1 byte
                long linesCount = fileSizeInBytes / charactersPerLine;

                for (int i = 0; i < linesCount; i++)
                {
                    var row = TextUtils.RandomAlphanumericString(charactersPerLine);
                    fileStream.WriteLine(row);
                }
            }
        }
    }
}