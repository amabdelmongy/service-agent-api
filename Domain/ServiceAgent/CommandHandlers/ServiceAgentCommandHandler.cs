using Domain.ServiceAgent.Aggregate;
using Domain.ServiceAgent.Commands;
using Domain.ServiceAgent.Events;

namespace Domain.ServiceAgent.CommandHandlers
{
    public interface IServiceAgentCommandHandler
    {
        Result<Event> Handle(ServiceAgentCommand command);
    }

    public class ServiceAgentCommandHandler : IServiceAgentCommandHandler
    {
        private readonly IServiceAgentService _serviceAgentService;
        private readonly IScheduleServiceAgentCommandHandler _requestCompleteServiceAgentCommandHandler;
        private readonly ICompleteServiceAgentCommandHandler _completeServiceAgentCommandHandler;
        private readonly IFailServiceAgentCommandHandler _failServiceAgentCommandHandler;
        public ServiceAgentCommandHandler(
            IServiceAgentService serviceAgentService,
            IScheduleServiceAgentCommandHandler requestCompleteServiceAgentCommandHandler,
            ICompleteServiceAgentCommandHandler completeServiceAgentCommandHandler, 
            IFailServiceAgentCommandHandler failServiceAgentCommandHandler)
        {
            _serviceAgentService = serviceAgentService;
            _requestCompleteServiceAgentCommandHandler = requestCompleteServiceAgentCommandHandler;
            _completeServiceAgentCommandHandler = completeServiceAgentCommandHandler;
            _failServiceAgentCommandHandler = failServiceAgentCommandHandler;
        }

        public Result<Event> Handle(ServiceAgentCommand command)
        {
            var serviceAgentResult = command is ScheduleServiceAgentCommand
                ? Result.Ok<ServiceAgentAggregate>(null)
                : _serviceAgentService.Get(command.ServiceAgentId);

            if (serviceAgentResult.HasErrors)
                return Result.Failed<Event>(serviceAgentResult.Errors);

            return command switch
            {
                ScheduleServiceAgentCommand scheduleServiceAgentCommand
                    => _requestCompleteServiceAgentCommandHandler.Handle(
                        scheduleServiceAgentCommand),

                CompleteServiceAgentCommand completeServiceAgentCommand
                    => _completeServiceAgentCommandHandler.Handle(
                            serviceAgentResult.Value,
                            completeServiceAgentCommand
                        ),

                FailServiceAgentCommand failServiceAgentCommand
                    => _failServiceAgentCommandHandler.Handle(
                            failServiceAgentCommand,
                            serviceAgentResult.Value.Version
                        ),

                _ => Result.Failed<Event>(
                    Error.CreateFrom(
                        "ServiceAgent Command Handler",
                        "Command not found")
                )
            };
        }
    }
}
