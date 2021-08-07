using System.Linq;
using Domain.ServiceAgent;
using Domain.ServiceAgent.Aggregate;
using Domain.ServiceAgent.CommandHandlers;
using Domain.ServiceAgent.Commands;
using Domain.ServiceAgent.Events;
using Domain.ServiceAgent.InputValidator;
using Moq;
using NUnit.Framework;

namespace Domain.Test
{
    public class ServiceAgentWorkFlowTests
    {
        [Test]
        public void WHEN_ServiceAgentInputValidator_has_error_THEN_return_Error()
        {
            var expectedError =
                Error.CreateFrom("subject", "message");

            var serviceAgentCommandHandlerMock = new Mock<IServiceAgentCommandHandler>();
            serviceAgentCommandHandlerMock
                .Setup(t =>
                    t.Handle(It.IsAny<ServiceAgentCommand>())
                )
                .Returns(Result.Ok<Event>());

            var inputValidatorMock = new Mock<IInputValidator>();
            inputValidatorMock
                .Setup(t =>
                    t.Validate(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()
                    )
                )
                .Returns(Result.Failed<object>(expectedError));

            var serviceAgentWorkflow =
                new ServiceAgentWorkflow(
                    serviceAgentCommandHandlerMock.Object,
                    inputValidatorMock.Object
                );

            var actual =
                serviceAgentWorkflow.Run(
                    ServiceAgentStubsTests.Name,
                    ServiceAgentStubsTests.ApiEndpoint,
                    ServiceAgentStubsTests.ApiEndpointActionString,
                    ServiceAgentStubsTests.Headers,
                    ServiceAgentStubsTests.Body,
                    ""
                );

            Assert.True(actual.HasErrors);
            Assert.AreEqual(1, actual.Errors.Count());
            Assert.AreEqual(expectedError.Subject, actual.Errors.First().Subject);
            Assert.AreEqual(expectedError.Message, actual.Errors.First().Message);
        }

        [Test]
        public void WHEN_ServiceAgentCommandHandler_has_error_THEN_return_Error()
        {
            var expectedError = Error.CreateFrom("subject", "message");

            var serviceAgentCommandHandlerMock = new Mock<IServiceAgentCommandHandler>();
            serviceAgentCommandHandlerMock
                .Setup(t =>
                    t.Handle(It.IsAny<ServiceAgentCommand>())
                )
                .Returns(Result.Failed<Event>(expectedError));

            var inputValidatorMock = new Mock<IInputValidator>();
            inputValidatorMock
                .Setup(t =>
                    t.Validate(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()
                        )
                )
                .Returns(Result.Ok<object>());

            var serviceAgentWorkflow =
                new ServiceAgentWorkflow(
                    serviceAgentCommandHandlerMock.Object,
                    inputValidatorMock.Object);

            var actual =
                serviceAgentWorkflow.Run(
                    ServiceAgentStubsTests.Name,
                    ServiceAgentStubsTests.ApiEndpoint,
                    ServiceAgentStubsTests.ApiEndpointActionString,
                    ServiceAgentStubsTests.Headers,
                    ServiceAgentStubsTests.Body,
                    "");

            Assert.True(actual.HasErrors);
            Assert.AreEqual(1, actual.Errors.Count());
            Assert.AreEqual(expectedError.Subject, actual.Errors.First().Subject);
            Assert.AreEqual(expectedError.Message, actual.Errors.First().Message);
        }
    }
}