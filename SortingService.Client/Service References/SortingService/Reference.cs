﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SortingService.Client.SortingService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SortingService.ISortingService")]
    public interface ISortingService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISortingService/BeginStream", ReplyAction="http://tempuri.org/ISortingService/BeginStreamResponse")]
        System.Guid BeginStream();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISortingService/BeginStream", ReplyAction="http://tempuri.org/ISortingService/BeginStreamResponse")]
        System.Threading.Tasks.Task<System.Guid> BeginStreamAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISortingService/PutStreamData", ReplyAction="http://tempuri.org/ISortingService/PutStreamDataResponse")]
        void PutStreamData(System.Guid streamGuid, string[] text);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISortingService/PutStreamData", ReplyAction="http://tempuri.org/ISortingService/PutStreamDataResponse")]
        System.Threading.Tasks.Task PutStreamDataAsync(System.Guid streamGuid, string[] text);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISortingService/GetSortedStream", ReplyAction="http://tempuri.org/ISortingService/GetSortedStreamResponse")]
        string[] GetSortedStream(System.Guid streamGuid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISortingService/GetSortedStream", ReplyAction="http://tempuri.org/ISortingService/GetSortedStreamResponse")]
        System.Threading.Tasks.Task<string[]> GetSortedStreamAsync(System.Guid streamGuid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISortingService/EndStream", ReplyAction="http://tempuri.org/ISortingService/EndStreamResponse")]
        void EndStream(System.Guid streamGuid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISortingService/EndStream", ReplyAction="http://tempuri.org/ISortingService/EndStreamResponse")]
        System.Threading.Tasks.Task EndStreamAsync(System.Guid streamGuid);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISortingServiceChannel : SortingService.ISortingService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SortingServiceClient : System.ServiceModel.ClientBase<SortingService.ISortingService>, SortingService.ISortingService {
        
        public SortingServiceClient() {
        }
        
        public SortingServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SortingServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SortingServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SortingServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Guid BeginStream() {
            return base.Channel.BeginStream();
        }
        
        public System.Threading.Tasks.Task<System.Guid> BeginStreamAsync() {
            return base.Channel.BeginStreamAsync();
        }
        
        public void PutStreamData(System.Guid streamGuid, string[] text) {
            base.Channel.PutStreamData(streamGuid, text);
        }
        
        public System.Threading.Tasks.Task PutStreamDataAsync(System.Guid streamGuid, string[] text) {
            return base.Channel.PutStreamDataAsync(streamGuid, text);
        }
        
        public string[] GetSortedStream(System.Guid streamGuid) {
            return base.Channel.GetSortedStream(streamGuid);
        }
        
        public System.Threading.Tasks.Task<string[]> GetSortedStreamAsync(System.Guid streamGuid) {
            return base.Channel.GetSortedStreamAsync(streamGuid);
        }
        
        public void EndStream(System.Guid streamGuid) {
            base.Channel.EndStream(streamGuid);
        }
        
        public System.Threading.Tasks.Task EndStreamAsync(System.Guid streamGuid) {
            return base.Channel.EndStreamAsync(streamGuid);
        }
    }
}
