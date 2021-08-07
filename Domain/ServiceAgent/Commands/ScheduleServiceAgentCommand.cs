using System;
using System.Collections.Generic;
using Domain.ServiceAgent.Aggregate;

namespace Domain.ServiceAgent.Commands
{
    public class ScheduleServiceAgentCommand : ServiceAgentCommand
    {
        public ScheduleServiceAgentCommand(
            string name,
            string apiEndpoint,
            ApiEndpointAction apiEndpointAction,
            IList<Header> headers,
            string body,
            Status status,
            DateTime submittedDate
        )
            : base(Guid.NewGuid())
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

