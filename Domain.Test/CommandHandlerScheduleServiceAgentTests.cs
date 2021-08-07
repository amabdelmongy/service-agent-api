using System.Linq;
using Domain.ServiceAgent.CommandHandlers;
using Domain.ServiceAgent.Commands;
using Domain.ServiceAgent.Events;
using Moq;
using NUnit.Framework;

namespace Domain.Test
{
    public class CommandHandlerScheduleServiceAgentTests
    {

        [Test]
        public void WHEN_Handle_ScheduleServiceAgentCommand_THEN_Create_ServiceAgentScheduledEvent_Event_and_return_Ok()
        {
            var eventRepository = new Mock<IEventRepository>();
            eventRepository
                    .Setup(repository => repository.Add(It.IsAny<Event>()))
                    .Returns(Result.Ok<object>());
             
            var requestCompleteServiceAgentCommandHandler = new ScheduleServiceAgentCommandHandler(eventRepository.Object);

            var scheduleServiceAgentCommand =
                ServiceAgentStubsTests.scheduleServiceAgentCommand;

            var actualResult = requestCompleteServiceAgentCommandHandler.Handle(scheduleServiceAgentCommand);

            Assert.IsTrue(actualResult.IsOk);
            var actual = actualResult.Value;
            Assert.AreEqual("ServiceAgentScheduledEvent", actual.Type);
            Assert.AreEqual(scheduleServiceAgentCommand.ServiceAgentId, actual.AggregateId);
            Assert.AreEqual(1, actual.Version);
            eventRepository.Verify(mock => mock.Add(It.IsAny<Event>()), Times.Once());
        }

        [Test]
        public void WHEN_Handle_ScheduleServiceAgentCommand_And_eventRepository_return_error_THEN_return_Error()
        {
            var expectedError =
                Error.CreateFrom("Error Subject", "Error Message");

            var eventRepositoryMock =
                new Mock<IEventRepository>();

            eventRepositoryMock
                .Setup(repository =>
                    repository.Add(It.IsAny<Event>())
                )
                .Returns(Result.Failed<object>(expectedError));

            var requestCompleteServiceAgentCommandHandler =
                new ScheduleServiceAgentCommandHandler(eventRepositoryMock.Object);

            var scheduleServiceAgentCommand =
                ServiceAgentStubsTests.scheduleServiceAgentCommand;

            var actualResult = requestCompleteServiceAgentCommandHandler.Handle(scheduleServiceAgentCommand);

            Assert.IsTrue(actualResult.HasErrors);
            Assert.AreEqual(1, actualResult.Errors.Count());
            var actualError = actualResult.Errors.First();

            Assert.AreEqual(expectedError.Subject, actualError.Subject);
            Assert.AreEqual(expectedError.Message, actualError.Message);

            eventRepositoryMock.Verify(mock => mock.Add(It.IsAny<Event>()), Times.Once());
        }
    }
}