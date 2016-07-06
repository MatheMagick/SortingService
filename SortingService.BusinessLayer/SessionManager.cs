using System;
using System.Collections.Generic;
using System.ServiceModel;
using SortingService.BusinessLayer.SortingAlgorithms;
using SortingService.DataAccess;

namespace SortingService.BusinessLayer
{
    public class SessionManager
    {
        private static readonly object CreationLock = new object();
        private static readonly Dictionary<Guid, object> SessionLocks = new Dictionary<Guid, object>();

        private readonly DataAccessLayer _dataAccess = new DataAccessLayer();
        private readonly ImprovedSorting _sortingAlgorithm = new ImprovedSorting();

        public Guid StartNewSession()
        {
            lock (CreationLock)
            {
                var result = _dataAccess.CreateNewSession();
                SessionLocks[result] = new object();

                return result;
            }
        }

        public void PutStreamData(Guid streamGuid, string[] text)
        {
            if(!_dataAccess.SessionExists(streamGuid))
            {
                throw new FaultException($"No session for the guid provided: {streamGuid}");
            }

            // Pesimistic concurrency is used for simplicity. Another option would be to use queues
            lock (SessionLocks[streamGuid])
            {
                // TODO The session may already be deleted
                string currentDataFile = _dataAccess.GetSortedSessionFile(streamGuid);
                var mergedFile = _sortingAlgorithm.Sort(text, currentDataFile);

                _dataAccess.SetSortedSessionFile(streamGuid, mergedFile);
            }
        }

        public string[] GetStreamData(Guid streamGuid)
        {
            if (!_dataAccess.SessionExists(streamGuid))
            {
                throw new FaultException($"No session for the guid provided: {streamGuid}");
            }

            lock (SessionLocks[streamGuid])
            {
                // TODO Getting the stream content is blocked by a Put operation. For responsiveness maybe have separate locks for reading and writing
                return _dataAccess.GetDataForSession(streamGuid);
            }
        }

        public void EndStream(Guid streamGuid)
        {
            if (!_dataAccess.SessionExists(streamGuid))
            {
                throw new FaultException($"No session for the guid provided: {streamGuid}");
            }

            lock (SessionLocks[streamGuid])
            {
                _dataAccess.DeleteSession(streamGuid);
            }
        }
    }
}