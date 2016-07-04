using System;
using System.ServiceModel;

namespace SortingService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ISortingService
    {
        [OperationContract]
        Guid BeginStream();

        [OperationContract]
        void PutStreamData(Guid streamGuid, string[] text);

        [OperationContract]
        string[] GetSortedStream(Guid streamGuid);

        [OperationContract]
        void EndStream(Guid streamGuid);
    }
}