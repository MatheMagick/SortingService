using System;
using System.Linq;

namespace SortingService.Client
{
    public static class TextUtils
    {
        private static Random random = new Random();

        public static string[] GetRandomAlphanumericData()
        {
            return Enumerable.Range(0, random.Next(20, 100))
                .Select(_ => RandomAlphanumericString(random.Next(30, 50)))
                .ToArray();
        }

        private static string RandomAlphanumericString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz 0123456789";
            return new string(Enumerable.Range(0, length)
                .Select(x => chars[random.Next(0, chars.Length - 1)])
                .ToArray());
        }
    }
}