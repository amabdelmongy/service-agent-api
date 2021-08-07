using System;

namespace Domain.ServiceAgentExecution
{
    public class RejectedServiceAgentExecutionError : Error
    {
        private RejectedServiceAgentExecutionError(
            Guid serviceAgentExecutionResultId,
            string subject,
            string message
        )
            : base(
                subject,
                null,
                message
            )
        {
            ServiceAgentExecutionResultId = serviceAgentExecutionResultId;
        }

        public Guid ServiceAgentExecutionResultId { get; }

        public static RejectedServiceAgentExecutionError CreateFrom(Guid serviceAgentExecutionResultId, string subject,
            string message = null)
        {
            return new RejectedServiceAgentExecutionError(serviceAgentExecutionResultId, subject, message);
        }
    }
}
