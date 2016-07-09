using System;

namespace SortingService.DataAccess
{
    public class UnfinishedChunkFileData
    {
        public string ChunkFileName { get; internal set; }
        public string ContentFileName { get; internal set; }
        public Guid SessionId { get; internal set; }
    }
}