using System;
using System.IO;

namespace SortingService.BusinessLayer
{
    public interface ISessionManager
    {
        /// <summary>
        /// Starts a new session with a unique id
        /// </summary>
        /// <returns>The unique identifier of the session</returns>
        Guid StartNewSession();

        /// <summary>
        /// Returns the accumulated and sorted data so far
        /// </summary>
        /// <param name="streamGuid">The unique identifier of the session</param>
        /// <returns></returns>
        Stream GetStreamData(Guid streamGuid);
        
        /// <summary>
        /// Merges the data for the id provided with the previous data and sorts it
        /// </summary>
        /// <param name="streamGuid">The unique identifier of the session</param>
        /// <param name="text">The data to add and sort</param>
        void PutStreamData(Guid streamGuid, string[] text);

        /// <summary>
        /// Ends the stream associated with the id provided, freeing up any resources taken
        /// </summary>
        /// <param name="streamGuid">The unique identifier of the session</param>
        void EndStream(Guid streamGuid);
    }
}