using System; 
using System.Linq;
using Domain.ServiceAgent;
using Domain.ServiceAgent.Aggregate;
using Domain.ServiceAgent.CommandHandlers;
using Domain.ServiceAgent.Commands;
using Domain.ServiceAgent.Events;
using Moq;
using NUnit.Framework;

namespace Domain.Test
{
    public class CommandHandlerServiceAgentTests
    {
        private Mock<IServiceAgentService> serviceAgentServiceMock()
        { 
            var serviceAgentServiceMock = new Mock<IServiceAgentService>();

            serviceAgentServiceMock
                .Setup(service =>
                    service.Get(It.IsAny<Guid>()))
                .Returns(Result.Ok<ServiceAgentAggregate>(ServiceAgentStubsTests.ServiceAgentAggregateTest()));
            return serviceAgentServiceMock;
        }
        private Mock<IScheduleServiceAgentCommandHandler> requestCompleteServiceAgentCommandHandlerMock()
        {
            var requestCompleteServiceAgentCommandHandlerMock = new Mock<IScheduleServiceAgentCommandHandler>();
            requestCompleteServiceAgentCommandHandlerMock
                .Setup(commandHandler =>
                    commandHandler.Handle(It.IsAny<ScheduleServiceAgentCommand>())
                )
                .Returns(It.IsAny<Result<Event>>());
            return requestCompleteServiceAgentCommandHandlerMock;
        }

        [Test]
        public void WHEN_handle_ScheduleServiceAgentCommand_THEN_should_call_requestCompleteServiceAgentCommandHandler()
        {  
            var requestCompleteServiceAgentCommandHandler = requestCompleteServiceAgentCommandHandlerMock();

            var serviceAgentCommandHandler =
                new ServiceAgentCommandHandler(
                    serviceAgentServiceMock().Object,
                    requestCompleteServiceAgentCommandHandler.Object,
                    null,
                    null
                );

            var scheduleServiceAgentCommand =
                ServiceAgentStubsTests.scheduleServiceAgentCommand;

            serviceAgentCommandHandler.Handle(scheduleServiceAgentCommand);

            requestCompleteServiceAgentCommandHandler.Verify(
                mock =>
                    mock.Handle(
                        scheduleServiceAgentCommand),
                Times.Once());
        }
    }
}