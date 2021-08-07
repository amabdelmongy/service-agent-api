using System.Threading.Tasks;
using Domain.ServiceAgent.Events;
using Domain.ServiceAgent.Projection;
using Newtonsoft.Json;

namespace Domain.MessageBus
{
    public interface IMessageBusHandler
    {
        Task Handle(Event serviceAgentEvent);
    }

    public class MessageBusHandler : IMessageBusHandler
    {
        private readonly IProjectionRepository _projectionRepository;

        public MessageBusHandler(IProjectionRepository projectionRepository)
        {
            _projectionRepository = projectionRepository;
        }

        public async Task Handle(Event serviceAgentEvent)
        {
            switch (serviceAgentEvent)
            {
                case ServiceAgentScheduledEvent @event:
                    await Handel(@event);
                    break;
                case ServiceAgentCompletedEvent @event:
                    await Handel(@event);
                    break;
                case ServiceAgentFailedEvent @event:
                    await Handel(@event);
                    break;
            }
        }

        private async Task Handel(ServiceAgentScheduledEvent serviceAgentScheduledEvent)
        {
            var serviceagentprojection = new ServiceAgentProjection
            {
                ServiceAgentId = serviceAgentScheduledEvent.AggregateId,
                Name = serviceAgentScheduledEvent.Name,
                ApiEndpoint = serviceAgentScheduledEvent.ApiEndpoint,
                ApiEndpointAction = serviceAgentScheduledEvent.ApiEndpointAction.ToString(),
                Body = serviceAgentScheduledEvent.Body,
                Headers = JsonConvert.SerializeObject(serviceAgentScheduledEvent.Headers),
                SubmittedDate = serviceAgentScheduledEvent.SubmittedDate,
                Status = serviceAgentScheduledEvent.Status.Id,
                ExecutionDate = null,
                LastUpdatedDate = serviceAgentScheduledEvent.TimeStamp
            };
            _projectionRepository.Add(serviceagentprojection);
        }
        private async Task Handel(ServiceAgentCompletedEvent @event)
        { 
            _projectionRepository.Update(@event);
        }
        private async Task Handel(ServiceAgentFailedEvent @event)
        {
            _projectionRepository.Update(@event);
        }
    }
}
