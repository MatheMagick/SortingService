using System;
using System.IO;
using SortingService.BusinessLayer;
using Microsoft.Practices.Unity;

namespace SortingService
{
    /// <summary>
    /// A service for streaming sorting of text files
    /// </summary>
    public class SortingService : ISortingService
    {
        // TODO Use IoC
        private static readonly ISessionManager _sessionManager;

        static SortingService()
        {
            // TODO Since this is a service library, we don't have a startup method so we use the static constructor
            // This is generally not a good idea, so once this is wrapped in an application move this to its startup method
            // There is also the Unity.WCF NuGet package, but sadly currently it does not play well with service libraries (it's targeting either IIS hosting or self-hosting)
            UnityConfig.InitializeBindings();
            _sessionManager = UnityConfig.Container.Resolve<ISessionManager>();
        }

        /// <summary>
        /// Begins a new stream
        /// </summary>
        /// <returns>The unique identifier of this session</returns>
        public Guid BeginStream()
        {
            return _sessionManager.StartNewSession();
        }

        /// <summary>
        /// Puts data in the stream associated with the Guid provided and sorts it. Always call BeginStream() beforehand to generate a new session
        /// </summary>
        /// <param name="streamGuid">The unique identifier of the session</param>
        /// <param name="text">The data to be added the session</param>
        public void PutStreamData(Guid streamGuid, string[] text)
        {
            _sessionManager.PutStreamData(streamGuid, text);
        }

        /// <summary>
        /// Gets the already sorted data for the stream associated with the Guid provided
        /// </summary>
        /// <param name="streamGuid">The unique identifier of the session</param>
        /// <returns>The data accumulated so far, sorted lexicographically line by line</returns>
        public Stream GetSortedStream(Guid streamGuid)
        {
            return _sessionManager.GetStreamData(streamGuid);
        }

        /// <summary>
        /// Ends the stream associated with the id provided, freeing up any resources taken
        /// </summary>
        /// <param name="streamGuid">The unique identifier of the session</param>
        public void EndStream(Guid streamGuid)
        {
            _sessionManager.EndStream(streamGuid);
        }
    }
}