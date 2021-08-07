using System;
using Domain.ServiceAgent.Aggregate;

namespace Domain.ServiceAgent.Events
{
    public class ServiceAgentFailedEvent : Event
    {
        public ServiceAgentFailedEvent(
            Guid aggregateId,
            DateTime timeStamp,
            int version,
            Guid? serviceAgentExecutionId,
            string details,
            Status status,
            DateTime executionDate)
            : base(
                aggregateId,
                timeStamp,
                version,
                typeof(ServiceAgentFailedEvent)
            )
        {
            ServiceAgentExecutionId = serviceAgentExecutionId;
            Details = details;
            Status = status;
            ExecutionDate = executionDate;
        }

        public Guid? ServiceAgentExecutionId { get; }

        public string Details { get; } 

        public Status Status { get; }

        public DateTime ExecutionDate { get; set; }
    }
}
