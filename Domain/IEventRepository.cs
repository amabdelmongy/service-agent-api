using System;
using System.Collections.Generic;
using Domain.ServiceAgent.Events;

namespace Domain
{
    public interface IEventRepository
    {
        Result<IEnumerable<Event>> Get(Guid id);
        Result<object> Add(Event @event);
    }
}
