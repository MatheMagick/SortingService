﻿using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using SortingService.BusinessLayer.SortingAlgorithms;
using SortingService.DataAccess;

namespace SortingService.BusinessLayer
{
    public class SessionManager
    {
        public static SessionManager Instance = new SessionManager();

        private static readonly object CreationLock = new object();
        // TODO Regenerate the locks upon startup
        private static readonly ConcurrentDictionary<Guid, object> SessionLocks = new ConcurrentDictionary<Guid, object>();

        private readonly DataAccessLayer _dataAccess = new DataAccessLayer();
        private readonly ImprovedSorting _sortingAlgorithm = new ImprovedSorting();

        private SessionManager()
        {
        }

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
            // Pesimistic concurrency is used for simplicity. Another option would be to use queues
            lock (SessionLocks.GetOrAdd(streamGuid, new object()))
            {
                CheckIfGuidIsValid(streamGuid);

                // TODO The session may already be deleted
                string currentDataFile = _dataAccess.GetSortedSessionFile(streamGuid);
                var mergedFile = _sortingAlgorithm.Sort(text, currentDataFile);

                _dataAccess.SetSortedSessionFile(streamGuid, mergedFile);
            }
        }

        public string[] GetStreamData(Guid streamGuid)
        {
            lock (SessionLocks.GetOrAdd(streamGuid, new object()))
            {
                CheckIfGuidIsValid(streamGuid);

                // TODO Getting the stream content is blocked by a Put operation. For responsiveness maybe have separate locks for reading and writing
                return _dataAccess.GetDataForSession(streamGuid);
            }
        }

        public void EndStream(Guid streamGuid)
        {
            lock(CreationLock)
            {
                lock (SessionLocks.GetOrAdd(streamGuid, new object()))
                {
                    CheckIfGuidIsValid(streamGuid);

                    _dataAccess.DeleteSession(streamGuid);

                    object removedKey;
                    SessionLocks.TryRemove(streamGuid, out removedKey);
                }
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