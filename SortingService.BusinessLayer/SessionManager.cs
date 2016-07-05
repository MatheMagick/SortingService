using System;
using System.ServiceModel;
using SortingService.BusinessLayer.SortingAlgorithms;
using SortingService.DataAccess;

namespace SortingService.BusinessLayer
{
    public class SessionManager
    {
        private DataAccessLayer _dataAccess = new DataAccessLayer();
        private ImprovedSorting _sortingAlgorithm = new ImprovedSorting();

        public Guid StartNewSession()
        {
            return _dataAccess.CreateNewSession();
        }

        public void PutStreamData(Guid streamGuid, string[] text)
        {
            if(!_dataAccess.SessionExists(streamGuid))
            {
                throw new FaultException($"No session for the guid provided: {streamGuid}");
            }

            string currentDataFile = _dataAccess.GetSortedSessionFile(streamGuid);

            var mergedData = _sortingAlgorithm.Sort(text, currentDataFile);

            _dataAccess.SetDataForSession(streamGuid, mergedData);
        }


        public string[] GetStreamData(Guid streamGuid)
        {
            if (!_dataAccess.SessionExists(streamGuid))
            {
                throw new FaultException($"No session for the guid provided: {streamGuid}");
            }

            return _dataAccess.GetDataForSession(streamGuid);
        }

        public void EndStream(Guid streamGuid)
        {
            if (!_dataAccess.SessionExists(streamGuid))
            {
                throw new FaultException($"No session for the guid provided: {streamGuid}");
            }

            _dataAccess.DeleteSession(streamGuid);
        }
    }
}