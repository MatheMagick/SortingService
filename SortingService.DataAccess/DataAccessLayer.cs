using System;
using System.Diagnostics;
using System.IO;

namespace SortingService.DataAccess
{
    /// <summary>
    /// The data access layer of the application
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

            using (File.Create(sessionFilePath))
            {
            }

            return result;
        }

        public bool SessionExists(Guid sessionGuid)
        {
            string sessionDirectoryPath = GetSessionDirectoryPath(sessionGuid);

            return Directory.Exists(sessionDirectoryPath);
        }

        public Stream GetDataForSession(Guid sessionGuid)
        {
            Debug.Assert(SessionExists(sessionGuid));

            string sessionFilePath = GetSessionContentFilePath(sessionGuid);

            return File.OpenRead(sessionFilePath);
        }

        public string GetSortedSessionFile(Guid sessionGuid)
        {
            Debug.Assert(SessionExists(sessionGuid));

            string sessionFilePath = GetSessionContentFilePath(sessionGuid);

            return sessionFilePath;
        }

        public void SetDataForSession(Guid sessionGuid, string[] data)
        {
            Debug.Assert(SessionExists(sessionGuid));

            string sessionFilePath = GetSessionContentFilePath(sessionGuid);

            File.WriteAllLines(sessionFilePath, data);
        }

        public string SetSortedSessionFile(Guid sessionGuid, string mergedFilePath)
        {
            Debug.Assert(SessionExists(sessionGuid));

            string sessionFilePath = GetSessionContentFilePath(sessionGuid);
            File.Delete(sessionFilePath);
            File.Move(mergedFilePath, sessionFilePath);
            return sessionFilePath;
        }

        public void DeleteSession(Guid sessionGuid)
        {
            string sessionDirectoryPath = GetSessionDirectoryPath(sessionGuid);

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