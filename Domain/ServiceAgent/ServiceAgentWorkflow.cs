using System;
using System.Collections.Generic;
using System.Linq;
using Domain.ServiceAgent.Aggregate;
using Domain.ServiceAgent.CommandHandlers;
using Domain.ServiceAgent.Commands;
using Domain.ServiceAgent.Events;
using Domain.ServiceAgent.InputValidator;
using Domain.ServiceAgentExecution;

namespace Domain.ServiceAgent
{
    public interface IServiceAgentWorkflow
    {
        Result<Event> Run(
            string name,
            string apiEndpoint,
            string apiEndpointAction,
            IList<Header> headers,
            string body,
            string submittedDate
        );
    }
    public class ServiceAgentWorkflow : IServiceAgentWorkflow
    {
        private readonly IServiceAgentCommandHandler _serviceAgentCommandHandler;
        private readonly IInputValidator _inputValidator;

        public ServiceAgentWorkflow(
            IServiceAgentCommandHandler serviceAgentCommandHandler
            , IInputValidator inputValidator)
        {
            _serviceAgentCommandHandler = serviceAgentCommandHandler;
            _inputValidator = inputValidator;
        }

        public Result<Event> Run(
            string name,
            string apiEndpoint,
            string apiEndpointAction,
            IList<Header> headers,
            string body,
            string submittedDate
        )
        {
            var validatestatus =
                _inputValidator.Validate(
                    name,
                    apiEndpoint,
                    apiEndpointAction
                );

            if (validatestatus.HasErrors)
                return Result.Failed <Event>(validatestatus.Errors);

            DateTime resultsubmittedDate = DateTime.Now;

            var isSchedule =
                !string.IsNullOrEmpty(submittedDate) && 
                DateTime.TryParse(submittedDate, out resultsubmittedDate);

            var serviceAgentScheduledEvent =
                _serviceAgentCommandHandler.Handle(
                    new ScheduleServiceAgentCommand(
                            name,
                            apiEndpoint,
                            Enum.Parse<ApiEndpointAction>(apiEndpointAction,true),
                            headers,
                            body,
                            Status.Scheduled,
                            resultsubmittedDate
                        )
                );

            if (serviceAgentScheduledEvent.HasErrors)
                return Result.Failed<Event>(serviceAgentScheduledEvent.Errors);

            if (!isSchedule)
            {
                var id = serviceAgentScheduledEvent.Value.AggregateId;

                var serviceAgentCompletedEvent =
                    _serviceAgentCommandHandler.Handle(
                        new CompleteServiceAgentCommand(id));

                if (serviceAgentCompletedEvent.HasErrors)
                {
                    var paymentErrors =
                        serviceAgentCompletedEvent.Errors
                            .Select(error =>
                                _serviceAgentCommandHandler.Handle(
                                    new FailServiceAgentCommand(
                                        id,
                                        error is RejectedServiceAgentExecutionError
                                            ? (Guid?)((RejectedServiceAgentExecutionError)error).ServiceAgentExecutionResultId
                                            : null,
                                        error.Message)
                                )
                            );

                    var errors = new List<Error>();
                    errors.AddRange(serviceAgentCompletedEvent.Errors);
                    errors.AddRange(paymentErrors.SelectMany(t => t.Errors).ToList());
                    return Result.Failed<Event>(errors);
                }
            }
            return serviceAgentScheduledEvent;
        }
    }
}
