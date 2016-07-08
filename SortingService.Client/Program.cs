﻿using System;
using System.Collections.Generic;

namespace SortingService.Client
{
    /// <summary>
    /// A test client for the <see cref="ISortingService"/> service
    /// </summary>
    class Program
    {
        static void Main()
        {
            new ServiceWrapperWIthAccumulation().SendRandomChunksAndCheckResult(3);

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}