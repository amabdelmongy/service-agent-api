using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.ServiceAgent.Events;
using Domain.ServiceAgent.Projection;

namespace WebApi.Integration.Test.ServiceAgentDetailsControllerTests
{
    public class InMemoryProjectionRepository : IProjectionRepository
    {
        readonly List<ServiceAgentProjection> _serviceAgentProjections = new List<ServiceAgentProjection>();

        private Result<ServiceAgentProjection> _resultGet;
        private Result<IEnumerable<ServiceAgentProjection>> _resultGetAll;
        private Result<object> _resultObject;

        public InMemoryProjectionRepository WithNewGetResult (Result<ServiceAgentProjection> resultGet)
        {
            _resultGet = resultGet;
            return this;
        }

        public Result<object> Add(ServiceAgentProjection serviceAgentProjection)
        {
             _serviceAgentProjections.Add(serviceAgentProjection);
             return _resultObject;
        }
        Result<ServiceAgentProjection> IProjectionRepository.Get(Guid id)
        {
            if (_resultGet != null) return _resultGet;
            return Result.Ok<ServiceAgentProjection>(
                _serviceAgentProjections
                    .FirstOrDefault(t =>
                    t.ServiceAgentId == id
                    )
            );
        }
        Result<IEnumerable<ServiceAgentProjection>> IProjectionRepository.Get(bool? isShowFavouriteOnly)
        {
            if (_resultGet != null) return _resultGetAll;
            return Result.Ok<IEnumerable<ServiceAgentProjection>>(
                _serviceAgentProjections
            ) ;
        }

        public Result<object> Update(ServiceAgentCompletedEvent @event)
        {
            return _resultObject;
        }

        public Result<object> Update(ServiceAgentFailedEvent @event)
        {
            return _resultObject;
        }

        public Result<object> UpdateIsFavourite(Guid id, bool isFavourite)
        {
            return _resultObject;
        }
    }
}

