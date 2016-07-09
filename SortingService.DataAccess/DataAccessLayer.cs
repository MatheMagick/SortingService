using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SortingService.DataAccess
{
    /// <summary>
    /// The data access layer of the application.
    /// This layer exists so in the future the file-based storage could be swapped for a SQL Server database
    /// The reasoning for choosing file-based storage is that it's easier to read, and in our case an SQL Server database would only bring further complexity without a significant benefit.
    /// If this is to be uploaded in Azure though an SQL DB is better
    /// </summary>
    public class DataAccessLayer
    {
        /// <summary>
        /// Creates a new session. A session consists of a folder with the GUID created and several files inside
        /// </summary>
        /// <returns>The unique id of the session</returns>
        public Guid CreateNewSession()
        {
            // Since the probability of a Guid crash is practically impossible (most DB engines rely on that), there's no need to double-check whether the Guid is already used.
            // For reference - http://stackoverflow.com/questions/2977593/is-it-safe-to-assume-a-guid-will-always-be-unique
            // In case this is a security concern, add a check whether the guid is already used
            var result = Guid.NewGuid();
            var sessionDirectoryPath = GetSessionDirectoryPath(result);
            // Creating the directory reserves this Guid
            Directory.CreateDirectory(sessionDirectoryPath);

            string sessionFilePath = GetSessionContentFilePath(result);

            // Create the content file
            using (File.Create(sessionFilePath))
            {
            }

            return result;
        }

        /// <summary>
        /// Returns whether a session exists
        /// </summary>
        /// <param name="sessionGuid">The unique identifier of the session</param>
        /// <returns></returns>
        public bool SessionExists(Guid sessionGuid)
        {
            string sessionDirectoryPath = GetSessionDirectoryPath(sessionGuid);

            return Directory.Exists(sessionDirectoryPath);
        }

        /// <summary>
        /// Returns the file path of the file that contains the sorted session data for the id provided
        /// </summary>
        /// <param name="sessionGuid">The unique identifier of the session</param>
        /// <returns></returns>
        public string GetSortedSessionFilePath(Guid sessionGuid)
        {
            Debug.Assert(SessionExists(sessionGuid));

            string sessionFilePath = GetSessionContentFilePath(sessionGuid);

            return sessionFilePath;
        }

        /// <summary>
        /// Returns all chunks that have not been still merged
        /// </summary>
        /// <returns></returns>
        public UnfinishedChunkFileData[] GetUnfinishedChunkFiles()
        {
            return Directory.GetFiles(Settings.Default.SessionDirectoriesRoot, "*.chunk", SearchOption.AllDirectories)
                .Select(x => new UnfinishedChunkFileData()
                {
                    ContentFileName = Path.ChangeExtension(x, ".txt"),
                    ChunkFileName = x,
                    SessionId = Guid.Parse(Path.GetDirectoryName(x))
                })
                .ToArray();
        }

        /// <summary>
        /// Swaps the session content file with the one provided as a parameter. This happens by deleting the previous content file and moving the one provided to its location
        /// </summary>
        /// <param name="sessionGuid">The unique identifier of the session</param>
        /// <param name="mergedFilePath">The file that should be the new content file</param>
        /// <returns></returns>
        public string SetSortedSessionFile(Guid sessionGuid, string mergedFilePath)
        {
            Debug.Assert(SessionExists(sessionGuid));

            string sessionFilePath = GetSessionContentFilePath(sessionGuid);
            File.Delete(sessionFilePath);
            File.Move(mergedFilePath, sessionFilePath);

            return sessionFilePath;
        }

        /// <summary>
        /// Deletes the session associated with the unique id provided and all its directories and files. Any attempt to access any operation on this unique id afterwards will result in an exception
        /// </summary>
        /// <param name="sessionGuid"></param>
        public void DeleteSession(Guid sessionGuid)
        {
            string sessionDirectoryPath = GetSessionDirectoryPath(sessionGuid);

            // This will not fail if the directory does not exist anymore
            Directory.Delete(sessionDirectoryPath, true);
        }

        private string GetSessionDirectoryPath(Guid sessionGuid)
        {
            return $"{Settings.Default.SessionDirectoriesRoot}\\{sessionGuid}\\";
        }

        private string GetSessionContentFilePath(Guid sessionGuid)
        {
            return GetSessionDirectoryPath(sessionGuid) + "content.txt";
        }
    }
}