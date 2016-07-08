using System;
using System.Collections.Concurrent;
using System.IO;
using System.ServiceModel;
using SortingService.BusinessLayer.SortingAlgorithms;
using SortingService.DataAccess;

namespace SortingService.BusinessLayer
{
    /// <summary>
    /// The business layer of the service
    /// </summary>
    public class SessionManager
    {
        // TODO Use IoC for singleton
        public static SessionManager Instance = new SessionManager();

        // Pesimistic concurrency is used for simplicity. Another option would be to use queues
        private static readonly ConcurrentDictionary<Guid, object> SessionLocks = new ConcurrentDictionary<Guid, object>();

        private readonly DataAccessLayer _dataAccess = new DataAccessLayer();
        private readonly ImprovedSorting _sortingAlgorithm = new ImprovedSorting();

        private SessionManager()
        {
        }

        /// <summary>
        /// Starts a new session with a unique id
        /// </summary>
        /// <returns>The unique identifier of the session</returns>
        public Guid StartNewSession()
        {
            return _dataAccess.CreateNewSession();
        }

        /// <summary>
        /// Merges the data for the id provided with the previous data and sorts it
        /// </summary>
        /// <param name="streamGuid">The unique identifier of the session</param>
        /// <param name="text">The data to add and sort</param>
        public void PutStreamData(Guid streamGuid, string[] text)
        {
            lock (SessionLocks.GetOrAdd(streamGuid, new object()))
            {
                CheckIfGuidIsValid(streamGuid);

                string currentDataFile = _dataAccess.GetSortedSessionFilePath(streamGuid);
                var mergedFile = _sortingAlgorithm.Sort(text, currentDataFile);

                _dataAccess.SetSortedSessionFile(streamGuid, mergedFile);
            }
        }

        /// <summary>
        /// Returns the accumulated and sorted data so far
        /// </summary>
        /// <param name="streamGuid">The unique identifier of the session</param>
        /// <returns></returns>
        public Stream GetStreamData(Guid streamGuid)
        {
            lock (SessionLocks.GetOrAdd(streamGuid, new object()))
            {
                // TODO This check is repeated almost everywhere. Aspects could be used to simplify this, but we don't currently have a license for PostSharp
                CheckIfGuidIsValid(streamGuid);

                // TODO Getting the stream content is blocked by a Put operation. For responsiveness maybe have separate locks for reading and writing
                var contentFilePath = _dataAccess.GetSortedSessionFilePath(streamGuid);
                return File.OpenRead(contentFilePath);
            }
        }

        public void EndStream(Guid streamGuid)
        {
            lock (SessionLocks.GetOrAdd(streamGuid, new object()))
            {
                CheckIfGuidIsValid(streamGuid);

                _dataAccess.DeleteSession(streamGuid);

                object removedKey;
                SessionLocks.TryRemove(streamGuid, out removedKey);
            }
        }

        private void CheckIfGuidIsValid(Guid streamGuid)
        {
            if (!_dataAccess.SessionExists(streamGuid))
            {
                throw new FaultException($"No session for the guid provided: {streamGuid}");
            }
        }
    }
}