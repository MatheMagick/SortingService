using System;
using System.Linq;

namespace SortingService.Client
{
    /// <summary>
    /// Utilities for random alphanumeric text generation
    /// </summary>
    public static class TextUtils
    {
        private static Random Random = new Random();
        private const string AlphanumericCharactersWithSpace = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz 0123456789";

        /// <summary>
        /// Creates an array of alphanumeric strings, each with random length in the range specified
        /// </summary>
        /// <param name="linesCount">How many lines should be in the data. Must be more than 0</param>
        /// <param name="minLineLengthm">Minimum length of each line</param>
        /// <param name="maxLineLength">Maximum length of each line</param>
        /// <returns></returns>
        public static string[] GetRandomAlphanumericData(int linesCount, int minLineLengthm, int maxLineLength)
        {
            if (linesCount < 1)
            {
                throw new ArgumentException("Lines count  must be more than 0", nameof(linesCount));
            }

            return Enumerable.Range(0, linesCount)
                .Select(_ => RandomAlphanumericString(Random.Next(minLineLengthm, maxLineLength)))
                .ToArray();
        }

        /// <summary>
        /// Creates a random alphanumeric string
        /// </summary>
        /// <param name="length">The resulting string's length. Must be more than 0</param>
        /// <returns></returns>
        public static string RandomAlphanumericString(int length)
        {
            if (length < 1)
            {
                throw new ArgumentException("length must be more than 0", nameof(length));
            }

            return new string(Enumerable.Range(0, length)
                .Select(x => AlphanumericCharactersWithSpace[Random.Next(0, AlphanumericCharactersWithSpace.Length - 1)])
                .ToArray());
        }
    }
}