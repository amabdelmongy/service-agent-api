using Domain.ServiceAgent.Events;

namespace Domain.MessageBus
{
    public class MessageSubscription
    {
        private readonly IMessageBusHandler _messageBusHandler;
        private readonly IServiceBusPublisher _serviceBusPublishers;

        public MessageSubscription(
            IServiceBusPublisher serviceBusPublishers,
            IMessageBusHandler messageBusHandler
        )
        {
            _serviceBusPublishers = serviceBusPublishers;
            _messageBusHandler = messageBusHandler;
        }
        public void AddSubscriptions()
        {
            _serviceBusPublishers.AddSubscription<ServiceAgentScheduledEvent>(_messageBusHandler.Handle);
            _serviceBusPublishers.AddSubscription<ServiceAgentCompletedEvent>(_messageBusHandler.Handle);
            _serviceBusPublishers.AddSubscription<ServiceAgentFailedEvent>(_messageBusHandler.Handle);
        }
    }
}
