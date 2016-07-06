using System;
using SortingService.BusinessLayer;

namespace SortingService
{
    /// <summary>
    /// A service for streaming sorting of text files
    /// </summary>
    public class SortingService : ISortingService
    {
        // TODO Use IoC
        // TODO On startup process all unfinished merges
        private SessionManager _sessionManager = new SessionManager();

        public Guid BeginStream()
        {
            return _sessionManager.StartNewSession();
        }

        public void PutStreamData(Guid streamGuid, string[] text)
        {
            _sessionManager.PutStreamData(streamGuid, text);
        }

        public string[] GetSortedStream(Guid streamGuid)
        {
            return _sessionManager.GetStreamData(streamGuid);
        }

        public void EndStream(Guid streamGuid)
        {
            _sessionManager.EndStream(streamGuid);
        }
    }
}