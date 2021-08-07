using System;
using System.Collections.Generic;
using System.Linq;
using Domain.ServiceAgent;
using Domain.ServiceAgent.Events;
using Moq;
using NUnit.Framework;

namespace Domain.Test
{
    public class ServiceAgentServiceTests
    {
        [Test]
        public void WHEN_eventRepository_has_error_THEN_return_Aggregate()
        {
            var expectedEvent = ServiceAgentStubsTests.ServiceAgentScheduledEventTest;
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock
                .Setup(t =>
                    t.Get(It.IsAny<Guid>())
                )
                .Returns(Result.Ok<IEnumerable<Event>>(
                    new List<Event>()
                    {
                        expectedEvent
                    })
                );

            var serviceAgentService =
                new ServiceAgentService(eventRepositoryMock.Object);

            var actual =
                serviceAgentService.Get(ServiceAgentStubsTests.ServiceAgentIdTest);

            Assert.True(actual.IsOk);
            Assert.AreEqual(expectedEvent.AggregateId, actual.Value.ServiceAgentId); 
        }

        [Test]
        public void WHEN_eventRepository_has_error_THEN_return_Error()
        {
            var expectedError = Error.CreateFrom("subject", "message");
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock
                .Setup(t =>
                    t.Get(It.IsAny<Guid>())
                )
                .Returns(Result.Failed<IEnumerable<Event>>(expectedError));

            var serviceAgentService = 
                new ServiceAgentService(eventRepositoryMock.Object);

            var actual = 
                serviceAgentService.Get(ServiceAgentStubsTests.ServiceAgentIdTest);

            Assert.True(actual.HasErrors);
            Assert.AreEqual(1, actual.Errors.Count());
            Assert.AreEqual(expectedError.Subject, actual.Errors.First().Subject);
            Assert.AreEqual(expectedError.Message, actual.Errors.First().Message);
        }

        [Test]
        public void WHEN_eventRepository_has_no_event_THEN_return_Error()
        {
            var eventRepositoryMock = new Mock<IEventRepository>();
            eventRepositoryMock
                .Setup(t =>
                    t.Get(It.IsAny<Guid>())
                )
                .Returns(Result.Ok<IEnumerable<Event>>(new List<Event>()));

            var serviceAgentService =
                new ServiceAgentService(eventRepositoryMock.Object);

            var actual =
                serviceAgentService.Get(ServiceAgentStubsTests.ServiceAgentIdTest);

            Assert.True(actual.HasErrors);
            Assert.AreEqual(1, actual.Errors.Count());
            Assert.AreEqual($"No ServiceAgentAggregate with serviceAgentId: {ServiceAgentStubsTests.ServiceAgentIdTest}", actual.Errors.First().Subject);
        }
    }
}
