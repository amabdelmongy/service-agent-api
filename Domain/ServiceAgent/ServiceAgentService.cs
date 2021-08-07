using System;
using System.Linq;
using Domain.ServiceAgent.Aggregate;

namespace Domain.ServiceAgent
{
    public interface IServiceAgentService
    {
        Result<ServiceAgentAggregate> Get(Guid serviceAgentId);
    }
    public class ServiceAgentService : IServiceAgentService
    {
        private readonly IEventRepository _events;

        public ServiceAgentService(IEventRepository events)
        {
            _events = events;
        }

        public Result<ServiceAgentAggregate> Get(Guid serviceAgentId)
        {
            var events = _events.Get(serviceAgentId);
            if (events.HasErrors)
                return Result.Failed<ServiceAgentAggregate>(events.Errors);

            if (!events.Value.Any()) 
                return Result.Failed<ServiceAgentAggregate>(Error.CreateFrom($"No ServiceAgentAggregate with serviceAgentId: { serviceAgentId }"));

            return AggregateFactory.CreateFrom(events.Value);
        }
    }
}
