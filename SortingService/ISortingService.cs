using System;
using System.IO;
using System.ServiceModel;

namespace SortingService
{
    /// <summary>
    /// A service for streaming sorting of text files
    /// </summary>
    [ServiceContract]
    public interface ISortingService
    {
        /// <summary>
        /// Begins a new stream
        /// </summary>
        /// <returns>The unique identifier of this session</returns>
        [OperationContract]
        Guid BeginStream();

        /// <summary>
        /// Puts data in the stream associated with the Guid provided and sorts it. Always call BeginStream() beforehand to generate a new session
        /// </summary>
        /// <param name="streamGuid">The unique identifier of the session</param>
        /// <param name="text">The data to be added the session</param>
        [OperationContract]
        void PutStreamData(Guid streamGuid, string[] text);

        /// <summary>
        /// Gets the already sorted data for the stream associated with the Guid provided
        /// </summary>
        /// <param name="streamGuid">The unique identifier of the session</param>
        /// <returns>The data accumulated so far, sorted lexicographically line by line</returns>
        [OperationContract]
        Stream GetSortedStream(Guid streamGuid);

        /// <summary>
        /// Ends the stream associated with the id provided, freeing up any resources taken
        /// </summary>
        /// <param name="streamGuid">The unique identifier of the session</param>
        [OperationContract]
        void EndStream(Guid streamGuid);
    }
}