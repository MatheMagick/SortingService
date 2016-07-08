using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SortingService.Client.ServiceReference1;

namespace SortingService.Client
{
    internal class ServiceWrapperWIthAccumulation
    {
        private List<string> AccumulatedData = new List<string>();

        internal void SendRandomChunksAndCheckResult(int chunksCount)
        {
            using (var client = new SortingServiceClient())
            {
                var sessionId = client.BeginStream();

                for (int i = 0; i < chunksCount; i++)
                {
                    bool chunkIsSortedCorrectly = SendChunkAndCheckResult(client, sessionId);

                    string status = chunkIsSortedCorrectly ? "OK" : "failed";
                    Console.WriteLine($"Sent chunk {i + 1} to the service. Equality comparison {status}");
                }

                client.EndStream(sessionId);
            }
        }

        private bool SendChunkAndCheckResult(SortingServiceClient client, Guid sessionId)
        {
            string[] chunk = TextUtils.GetRandomAlphanumericData(100, 20,50);

            client.PutStreamData(sessionId, chunk);
            IEnumerable<string> resultData = GetSortedDataFromService(client, sessionId);

            AccumulatedData.AddRange(chunk);
            // The default string comparer is lexicographical so it's safe to use
            AccumulatedData.Sort();

            return AccumulatedData.SequenceEqual(resultData);
        }

        private static IEnumerable<string> GetSortedDataFromService(SortingServiceClient client, Guid sessionId)
        {
            List<string> resultData = new List<string>();

            using (var result = client.GetSortedStream(sessionId))
            using (var resultReader = new StreamReader(result))
            {
                string resultLine;

                while ((resultLine = resultReader.ReadLine()) != null)
                {
                    resultData.Add(resultLine);
                }
            }

            return resultData;
        }
    }
}