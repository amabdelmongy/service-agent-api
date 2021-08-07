using System.Collections.Generic;
using Domain.ServiceAgent.Aggregate;
using Domain.ServiceAgent.Events;
using NUnit.Framework;

namespace Domain.Test
{
    public class AggregateFactoryTests
    {
        [Test]
        public void WHEN_pass_ServiceAgentScheduledEvent_THEN_return_correct_Aggregate()
        { 
            var expectedEvent = ServiceAgentStubsTests.ServiceAgentScheduledEventTest;
            var serviceAgentAggregateResult =
                AggregateFactory.CreateFrom(
                    new List<Event>
                    {
                        expectedEvent
                    }
                );

            Assert.True(serviceAgentAggregateResult.IsOk);
            var actualAggregate = serviceAgentAggregateResult.Value;

            Assert.AreEqual(expectedEvent.AggregateId, actualAggregate.ServiceAgentId);
            Assert.AreEqual(expectedEvent.Version, actualAggregate.Version);
            Assert.AreEqual(expectedEvent.ApiEndpoint, actualAggregate.ApiEndpoint);
            Assert.AreEqual(expectedEvent.ApiEndpointAction, actualAggregate.ApiEndpointAction);
            Assert.AreEqual(expectedEvent.Body, actualAggregate.Body);
            Assert.AreEqual(expectedEvent.Headers, actualAggregate.Headers);
            Assert.AreEqual(expectedEvent.Name, actualAggregate.Name);
            Assert.AreEqual(expectedEvent.Status, actualAggregate.Status);
            Assert.AreEqual(expectedEvent.SubmittedDate, actualAggregate.SubmittedDate);
        }
    }
}
