using System;
using System.Diagnostics;
using System.IO;

namespace SortingService.DataAccess
{
    public class DataAccessLayer
    {
        public Guid CreateNewSession()
        {
            Guid result = Guid.NewGuid();

            string sessionDirectoryPath = GetSessionDirectoryPath(result);

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

        public string[] GetDataForSession(Guid sessionGuid)
        {
            Debug.Assert(SessionExists(sessionGuid));
            string sessionFilePath = GetSessionContentFilePath(sessionGuid);

            return File.ReadAllLines(sessionFilePath);
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