using System;
using System.Collections.Generic;
using System.Linq;
using Domain.ServiceAgent.Events;

namespace Domain.ServiceAgent.Aggregate
{
    public static class AggregateFactory
    {
        public static Result<ServiceAgentAggregate> CreateFrom(IEnumerable<Event> events)
        {
            var resultServiceAgent =
                events
                    .OrderBy(x => x.Version)
                    .ToList()
                    .Aggregate(new ServiceAgentAggregate(), (serviceAgentAggregate, e) =>
                    {
                        switch (e)
                        {
                            case ServiceAgentScheduledEvent @event:
                                serviceAgentAggregate =
                                    serviceAgentAggregate.With(
                                        @event.AggregateId,
                                        @event.Version,
                                        @event.Name,
                                        @event.ApiEndpoint,
                                        @event.ApiEndpointAction,
                                        @event.Headers,
                                        @event.Body,
                                        @event.SubmittedDate,
                                        @event.Status
                                    );
                                break;

                            case ServiceAgentCompletedEvent @event:
                                serviceAgentAggregate =
                                    serviceAgentAggregate.With(
                                        @event.Status,
                                        @event.Version,
                                        @event.ExecutionDate,
                                        @event.ResponseBody,
                                        @event.ResponseHeaders);
                                break;

                            case ServiceAgentFailedEvent @event:
                                serviceAgentAggregate =
                                    serviceAgentAggregate.With(
                                        @event.Status,
                                        @event.Version,
                                        @event.ExecutionDate);
                                break;


                            default:
                                throw new NotSupportedException();
                        }

                        return serviceAgentAggregate;
                    });

            return Result.Ok(resultServiceAgent);
        }
    }
}
