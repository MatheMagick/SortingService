using System;

namespace SortingService.DataAccess
{
    public interface IDataAccessLayer
    {
        /// <summary>
        /// Creates a new session. A session consists of a folder with the GUID created and several files inside
        /// </summary>
        /// <returns>The unique id of the session</returns>
        Guid CreateNewSession();

        /// <summary>
        /// Returns whether a session exists
        /// </summary>
        /// <param name="sessionGuid">The unique identifier of the session</param>
        /// <returns></returns>
        bool SessionExists(Guid sessionGuid);

        /// <summary>
        /// Returns the file path of the file that contains the sorted session data for the id provided
        /// </summary>
        /// <param name="sessionGuid">The unique identifier of the session</param>
        /// <returns></returns>
        string GetSortedSessionFilePath(Guid sessionGuid);

        /// <summary>
        /// Returns all chunks that have not been still merged
        /// </summary>
        /// <returns></returns>
        UnfinishedChunkFileData[] GetUnfinishedChunkFiles();

        /// <summary>
        /// Swaps the session content file with the one provided as a parameter. This happens by deleting the previous content file and moving the one provided to its location
        /// </summary>
        /// <param name="sessionGuid">The unique identifier of the session</param>
        /// <param name="mergedFilePath">The file that should be the new content file</param>
        /// <returns></returns>
        string SetSortedSessionFile(Guid sessionGuid, string mergedFilePath);

        /// <summary>
        /// Deletes the session associated with the unique id provided and all its directories and files. Any attempt to access any operation on this unique id afterwards will result in an exception
        /// </summary>
        /// <param name="sessionGuid"></param>
        void DeleteSession(Guid sessionGuid);
    }
}