using System.Threading.Tasks;
using Domain.MessageBus;
using Domain.ServiceAgent.Aggregate;
using Domain.ServiceAgent.Events;
using Domain.ServiceAgent.Projection;
using Moq;
using NUnit.Framework;

namespace Domain.Test
{
    class MessageSubscriptionHandlerTests
    {
        [Test]
        public async Task WHEN_pass_ServiceAgentScheduledEvent_THEN_call_ProjectionRepository_Add()
        {
            var serviceAgentScheduledEvent = ServiceAgentStubsTests.ServiceAgentScheduledEventTest;
            var expectedServiceAgentProjection = new ServiceAgentProjection
            {
                ServiceAgentId = serviceAgentScheduledEvent.AggregateId,
                Name = serviceAgentScheduledEvent.Name,
                ApiEndpoint = serviceAgentScheduledEvent.ApiEndpoint,
                ApiEndpointAction = serviceAgentScheduledEvent.ApiEndpointAction.ToString(),
                Body = serviceAgentScheduledEvent.Body,
                SubmittedDate = serviceAgentScheduledEvent.SubmittedDate,
                Status = Status.Scheduled.ToString(),
                ExecutionDate = null,
                LastUpdatedDate = serviceAgentScheduledEvent.TimeStamp
            };

            var projectionRepositoryMock = new Mock<IProjectionRepository>();
            projectionRepositoryMock
                .Setup(repository =>
                    repository.Add(expectedServiceAgentProjection))
                .Returns(Result.Ok<object>());

            var messageBusHandler = new MessageBusHandler(projectionRepositoryMock.Object);

            var a = messageBusHandler.Handle(serviceAgentScheduledEvent);


            projectionRepositoryMock.Verify(
                t => t.Add(
                    It.IsAny<ServiceAgentProjection>()
                ), Times.Once);
        }
    }
}
