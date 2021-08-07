using System;

namespace Domain.ServiceAgent.Commands
{
    public abstract class ServiceAgentCommand
    {
        public Guid ServiceAgentId { get; }
        protected ServiceAgentCommand(Guid serviceAgentId)
        {
            ServiceAgentId = serviceAgentId;
        }
    }
}
