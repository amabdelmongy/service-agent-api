using System;

namespace Domain.ServiceAgent.Commands
{
    public class CompleteServiceAgentCommand : ServiceAgentCommand
    {
        public CompleteServiceAgentCommand(
            Guid serviceAgentId
        )
            : base(serviceAgentId)
        {
        }
    }
}
