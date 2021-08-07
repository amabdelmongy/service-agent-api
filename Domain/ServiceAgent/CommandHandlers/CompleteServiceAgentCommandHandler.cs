using System;
using Domain.ServiceAgentExecution;
using Domain.ServiceAgent.Aggregate;
using Domain.ServiceAgent.Commands;
using Domain.ServiceAgent.Events;

namespace Domain.ServiceAgent.CommandHandlers
{
    public interface ICompleteServiceAgentCommandHandler
    {
        Result<Event> Handle(
            ServiceAgentAggregate serviceAgentAggregate,
            CompleteServiceAgentCommand completeServiceAgentCommand
        );
    }

    public class CompleteServiceAgentCommandHandler : ICompleteServiceAgentCommandHandler
    {
        private readonly IEventRepository _eventRepository;
        private readonly IExecutionFacade _executionFacade;

        public CompleteServiceAgentCommandHandler(
            IEventRepository eventRepository,
            IExecutionFacade executionFacade
        )
        {
            _eventRepository = eventRepository;
            _executionFacade = executionFacade;
        }

        public Result<Event> Handle(
            ServiceAgentAggregate serviceAgentAggregate,
            CompleteServiceAgentCommand completeServiceAgentCommand
        )
        {
            var serviceAgentExecutionResult =
                _executionFacade.ProcessExecution(
                    serviceAgentAggregate
                );

            if (serviceAgentExecutionResult.HasErrors)
                return Result.Failed<Event>(serviceAgentExecutionResult.Errors);

            var result = serviceAgentExecutionResult.Value;
            var serviceAgentExecutionServiceAgentCompletedEvent =
                new ServiceAgentCompletedEvent(
                    completeServiceAgentCommand.ServiceAgentId,
                    DateTime.Now,
                    serviceAgentAggregate.Version + 1,
                    Status.Completed,
                    result.ResponseBody,
                    result.ResponseHeaders,
                    result.ExecutionDate
                );

            var resultRepository =
                _eventRepository
                    .Add(serviceAgentExecutionServiceAgentCompletedEvent);

            return
                resultRepository.IsOk
                    ? Result.Ok((Event) serviceAgentExecutionServiceAgentCompletedEvent)
                    : Result.Failed<Event>(resultRepository.Errors);
        }
    }
}