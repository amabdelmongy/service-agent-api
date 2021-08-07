using System;
using System.Collections.Generic;
using Domain.ServiceAgent.Aggregate;
using Newtonsoft.Json;

namespace Domain.ServiceAgent.Events
{
    public class ServiceAgentCompletedEvent : Event
    {
        [JsonConstructor]
        public ServiceAgentCompletedEvent(
            Guid aggregateId,
            DateTime timeStamp,
            int version,
            Status status,
            string responseBody,
            IList<Header> responseHeaders,
            DateTime executionDate)
            : base(
                aggregateId,
                timeStamp,
                version,
                typeof(ServiceAgentCompletedEvent)
            )
        {
            Status = status;
            ResponseHeaders = responseHeaders;
            ExecutionDate = executionDate;
            ResponseBody = responseBody;
        }
        public string ResponseBody { get; set; }
        public IList<Header> ResponseHeaders { get; set; }
        public DateTime ExecutionDate { get; set; } 
        public Status Status { get; }
    }
}
