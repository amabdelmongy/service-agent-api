using System;
using Domain.ServiceAgent.Commands;
using Domain.ServiceAgent.Events;

namespace Domain.ServiceAgent.CommandHandlers
{
    public interface IScheduleServiceAgentCommandHandler
    {
        Result<Event> Handle(ScheduleServiceAgentCommand scheduleServiceAgentCommand);
    }
    public class ScheduleServiceAgentCommandHandler : IScheduleServiceAgentCommandHandler
    {
        private readonly IEventRepository _eventRepository;

        public ScheduleServiceAgentCommandHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Result<Event> Handle(ScheduleServiceAgentCommand scheduleServiceAgentCommand)
        {
            var serviceAgentScheduledEvent =
                new ServiceAgentScheduledEvent(
                    scheduleServiceAgentCommand.ServiceAgentId,
                    DateTime.Now,
                    1,
                    scheduleServiceAgentCommand.Name,
                    scheduleServiceAgentCommand.ApiEndpoint,
                    scheduleServiceAgentCommand.ApiEndpointAction,
                    scheduleServiceAgentCommand.Headers,
                    scheduleServiceAgentCommand.Body,
                    scheduleServiceAgentCommand.SubmittedDate,
                    scheduleServiceAgentCommand.Status
                );

            var result = _eventRepository.Add(serviceAgentScheduledEvent);

            return
                result.IsOk
                    ? Result.Ok((Event) serviceAgentScheduledEvent)
                    : Result.Failed<Event>(result.Errors);
        }
    }
}
