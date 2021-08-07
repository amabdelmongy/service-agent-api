using System;
using System.Collections.Generic;
using Domain.ServiceAgent.Events;

namespace Domain.ServiceAgent.Projection
{
    public interface IProjectionRepository
    {
        Result<ServiceAgentProjection> Get(Guid id);
        Result<IEnumerable<ServiceAgentProjection>> Get(bool? isShowFavouriteOnly);
        Result<object> Add(ServiceAgentProjection serviceAgentProjection);
        Result<object> Update(ServiceAgentCompletedEvent @event);
        Result<object> Update(ServiceAgentFailedEvent @event);
        Result<object> UpdateIsFavourite(Guid id, bool isFavourite);
    }
}