using System;
using Domain.ServiceAgent.Aggregate;
using Domain.ServiceAgent.Commands;
using Domain.ServiceAgent.Events;

namespace Domain.ServiceAgent.CommandHandlers
{
    public interface IFailServiceAgentCommandHandler
    {
        Result<Event> Handle(
            FailServiceAgentCommand failServiceAgentCommand,
            int version
        );
    }
    public class FailServiceAgentCommandHandler : IFailServiceAgentCommandHandler
    {
        private readonly IEventRepository _eventRepository;

        public FailServiceAgentCommandHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        public Result<Event> Handle(
            FailServiceAgentCommand failServiceAgentCommand,
            int version
        )
        {
            var serviceAgentExecutionServiceAgentFailedEvent =
                new ServiceAgentFailedEvent(
                    failServiceAgentCommand.ServiceAgentId,
                    DateTime.Now,
                    version + 1,
                    failServiceAgentCommand.ServiceAgentExecutionId,
                    failServiceAgentCommand.Details,
                    Status.Failed,
                    DateTime.Now
                );

            var result = _eventRepository.Add(serviceAgentExecutionServiceAgentFailedEvent);

            return
                result.IsOk
                    ? Result.Ok((Event) serviceAgentExecutionServiceAgentFailedEvent)
                    : Result.Failed<Event>(result.Errors);
        }
    }
}