using System;

namespace Domain.ServiceAgent.Commands
{
    public class FailServiceAgentCommand : ServiceAgentCommand
    {
        public FailServiceAgentCommand(
            Guid serviceAgentId,
            Guid? serviceAgentExecutionId,
            string details
        )
            : base(serviceAgentId)
        {
            ServiceAgentExecutionId = serviceAgentExecutionId;
            Details = details;
        }

        public Guid? ServiceAgentExecutionId { get; }
        public string Details { get; }
    }
}
