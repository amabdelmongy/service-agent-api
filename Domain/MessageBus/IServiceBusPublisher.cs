using System;
using System.Threading.Tasks;
using Domain.ServiceAgent.Events;

namespace Domain.MessageBus
{
    public interface IServiceBusPublisher
    {
        void Publish<T>(T @event) where T : Event;
        void AddSubscription<T>(Func<Event, Task> handler);
    }
}
