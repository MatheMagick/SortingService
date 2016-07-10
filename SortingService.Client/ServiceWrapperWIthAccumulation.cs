using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SortingService.Client.ServiceReference1;
using SortingService.Clients.Common;

namespace SortingService.Client
{
    /// <summary>
    /// A wrapper for the <see cref="ISortingService"/> service that sends multiple chunks to it and checks whether the result returned is correct
    /// </summary>
    internal class ServiceWrapperWIthAccumulation
    {
        private readonly List<string> _accumulatedData = new List<string>();

        /// <summary>
        /// Sends multiple random alphanumerical chunks to the service and checks whether the result returned is correct
        /// </summary>
        /// <param name="chunksCount">How many chunks should be sent</param>
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

            IEnumerable<string> expectedData = GetExpectedData(chunk);
            IEnumerable<string> resultData = GetSortedDataFromService(client, sessionId);

            return expectedData.SequenceEqual(resultData);
        }

        private IEnumerable<string> GetExpectedData(string[] chunk)
        {
            // Store the data sent so far in sorted order
            _accumulatedData.AddRange(chunk);
            // The default string comparer is lexicographical so it's safe to use
            _accumulatedData.Sort();

            return _accumulatedData;
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