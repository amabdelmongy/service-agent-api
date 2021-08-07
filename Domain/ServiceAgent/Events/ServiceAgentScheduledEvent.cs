using System;
using System.Collections.Generic;
using Domain.ServiceAgent.Aggregate;

namespace Domain.ServiceAgent.Events
{
    public class ServiceAgentScheduledEvent : Event
    {
        public ServiceAgentScheduledEvent(
            Guid aggregateId,
            DateTime timeStamp,
            int version,
            string name,
            string apiEndpoint,
            ApiEndpointAction apiEndpointAction,
            IList<Header> headers,
            string body,
            DateTime submittedDate,
            Status status
        )
            : base(
                aggregateId,
                timeStamp,
                version,
                typeof(ServiceAgentScheduledEvent)
            )
        {
            Name = name;
            ApiEndpoint = apiEndpoint;
            ApiEndpointAction = apiEndpointAction;
            Headers = headers;
            Body = body;
            SubmittedDate = submittedDate;
            Status = status;
        }
        public string Name { get; }
        public string ApiEndpoint { get; }
        public ApiEndpointAction ApiEndpointAction { get; }
        public IList<Header> Headers { get; }
        public string Body { get; }
        public DateTime SubmittedDate { get; }
        public Status Status { get; }
    }
}