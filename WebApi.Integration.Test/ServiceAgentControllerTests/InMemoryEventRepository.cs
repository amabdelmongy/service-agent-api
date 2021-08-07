﻿using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.ServiceAgent.Events;

namespace WebApi.Integration.Test.ServiceAgentControllerTests
{
    public class InMemoryEventRepository : IEventRepository
    {
        readonly List<Tuple<Guid, Event>> _tuples = new List<Tuple<Guid, Event>>();

        private Result<object> _addResult;

        public InMemoryEventRepository WithNewAdd(Result<object> result)
        {
            _addResult = result;
            return this;
        }

        public Result<object> Add(Event @event)
        {
            if (_addResult != null) return _addResult;

            _tuples.Add(new Tuple<Guid, Event>(@event.AggregateId, @event));
            return Result.Ok<object>();
        }

        private Result<IEnumerable<Event>> _getResult;

        public InMemoryEventRepository WithNewGet(Result<IEnumerable<Event>> result)
        {
            _getResult = result;
            return this;
        }

        public Result<IEnumerable<Event>> Get(Guid id)
        {
            if (_getResult != null) return _getResult;

            var events =
                _tuples
                    .Where(t => t.Item1 == id)
                    .Select(t => t.Item2);

            return
                Result.Ok(events);
        }
    }
}

