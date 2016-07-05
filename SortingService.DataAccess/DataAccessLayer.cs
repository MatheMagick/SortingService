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

            string sessionFilePath = GetSessionFilePath(result);

            using (var fileStream = File.Create(sessionFilePath))
            {
            }

            return result;
        }

        public bool SessionExists(Guid sessionGuid)
        {
            string sessionFilePath = GetSessionFilePath(sessionGuid);

            return File.Exists(sessionFilePath);
        }

        public string[] GetDataForSession(Guid sessionGuid)
        {
            Debug.Assert(SessionExists(sessionGuid));
            string sessionFilePath = GetSessionFilePath(sessionGuid);

            return File.ReadAllLines(sessionFilePath);
        }

        public string GetSortedSessionFile(Guid sessionGuid)
        {
            Debug.Assert(SessionExists(sessionGuid));
            string sessionFilePath = GetSessionFilePath(sessionGuid);

            return sessionFilePath;
        }

        public void SetDataForSession(Guid sessionGuid, string[] data)
        {
            Debug.Assert(SessionExists(sessionGuid));
            string sessionFilePath = GetSessionFilePath(sessionGuid);

            File.WriteAllLines(sessionFilePath, data);
        }

        private string GetSessionFilePath(Guid sessionGuid)
        {
            return $"{Settings.Default.SessionDirectoriesRoot}\\{sessionGuid.ToString()}.srt";
        }

        public void DeleteSession(Guid sessionGuid)
        {
            string sessionFilePath = GetSessionFilePath(sessionGuid);

            File.Delete(sessionFilePath);
        }
    }
}