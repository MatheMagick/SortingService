using System;
using System.Collections.Generic;
using System.Linq;
using SortingService.Client.SortingService;

namespace SortingService.Client
{
    class Program
    {
        private static Random random = new Random();
        private static int triesCount = 0;
        private static List<string> accumulatedData = new List<string>();

        static void Main()
        {
            using (var client = new SortingServiceClient())
            {
                var sessionId = client.BeginStream();

                for (int i = 0; i < 3; i++)
                {
                    SendChunk(client, sessionId);
                }

                client.EndStream(sessionId);

                //try
                //{
                //    SendChunk(client, sessionId);
                //}
                //catch (FaultException)
                //{
                //    // It is expected to get a FaultException in this case
                //    Console.WriteLine("Trying to access closed session results in expected behavior");
                //}
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void SendChunk(SortingServiceClient client, Guid sessionId)
        {
            string[] chunk = TextUtils.GetRandomAlphanumericData();

            client.PutStreamData(sessionId, chunk);
            var result = client.GetSortedStream(sessionId);

            accumulatedData.AddRange(chunk);
            // The default string comparer is lexicographical so it's safe to use
            accumulatedData.Sort();

            bool isCorrect = accumulatedData.SequenceEqual(result);
            string status = isCorrect ? "OK" : "failed";
            ++triesCount;

            Console.WriteLine($"Sent chunk {triesCount} to the service. Equality comparison {status}");
        }
    }
}