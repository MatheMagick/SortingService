using System;
using System.IO;
using SortingService.Client;

namespace SortingService.Client16GBFile
{
    internal class LargeFileGenerator
    {
        private const string FileName = "large.txt";

        public string CreateLargeFile(byte sizeInGB, int charactersPerLine)
        {
            if(sizeInGB == 0)
            {
                throw new ArgumentException("Size in GB must be more than 0", nameof(sizeInGB));
            }

            if (charactersPerLine < 10 || charactersPerLine > 10000)
            {
                throw new ArgumentException("Characters per line  must be between than 10 and 10000", nameof(charactersPerLine));
            }

            long fileSizeInBytes = (long)sizeInGB * 1000 * 1000 * 1000; 

            if( !FileExistsAndHasCorrectSize(fileSizeInBytes) )
            {
                CreateRandomTextFile(fileSizeInBytes, charactersPerLine);
            }

            return FileName;
        }

        private bool FileExistsAndHasCorrectSize(long fileSizeInBytes)
        {
            return File.Exists(FileName) &&
                   // Allow for 10% size error
                   new FileInfo(FileName).Length > fileSizeInBytes * 0.9 &&
                   new FileInfo(FileName).Length < fileSizeInBytes * 1.1;
        }

        private void CreateRandomTextFile(long fileSizeInBytes, int charactersPerLine)
        {
            using (var fileStream = File.CreateText(FileName))
            {
                // Since we use UTF8 / ASCII each character is 1 byte
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