using System.Linq;
using Domain.ServiceAgent.Aggregate;
using Domain.ServiceAgent.InputValidator;
using NUnit.Framework;

namespace Domain.Test
{
    class InputValidatorTests
    {
        [Test]
        public void WHEN_inputs_has_no_error_THEN_return_Ok()
        { 
            var actual = new InputValidator().Validate(
                ServiceAgentStubsTests.Name,
                ServiceAgentStubsTests.ApiEndpoint,
                ServiceAgentStubsTests.ApiEndpointActionString
                );

            Assert.True(actual.IsOk); 
        }

        [Test]
        public void WHEN_name_empty_THEN_return_Error()
        {
            var actual = new InputValidator().Validate(
                "",
                ServiceAgentStubsTests.ApiEndpoint,
                ServiceAgentStubsTests.ApiEndpointActionString
                );

            Assert.True(actual.HasErrors);
            Assert.AreEqual(1, actual.Errors.Count());
            Assert.AreEqual("Invalid Name", actual.Errors.First().Subject);
            Assert.AreEqual("Name is Empty", actual.Errors.First().Message);
        }

        [Test]
        public void WHEN_ApiEndpoint_empty_THEN_return_Error()
        {
            var actual = new InputValidator().Validate(
                ServiceAgentStubsTests.Name,
                "",
                ServiceAgentStubsTests.ApiEndpointActionString
                );

            Assert.True(actual.HasErrors);
            Assert.AreEqual(1, actual.Errors.Count());
            Assert.AreEqual("Invalid Api end point", actual.Errors.First().Subject);
            Assert.AreEqual("Api end point is Empty", actual.Errors.First().Message);
        }

        [Test]
        public void WHEN_ApiEndpointAction_empty_THEN_return_Error()
        {
            var actual = new InputValidator().Validate(
                ServiceAgentStubsTests.Name,
                ServiceAgentStubsTests.ApiEndpoint,
                ""
                );

            Assert.True(actual.HasErrors);
            Assert.AreEqual(1, actual.Errors.Count());
            Assert.AreEqual("Invalid Api end point action", actual.Errors.First().Subject);
            Assert.AreEqual("Api end point action is Empty", actual.Errors.First().Message);
        }

        [Test]
        public void WHEN_ApiEndpointAction_is_invalid_THEN_return_Error()
        {
            var actual = new InputValidator().Validate(
                ServiceAgentStubsTests.Name,
                ServiceAgentStubsTests.ApiEndpoint,
                "invalid"
                );

            Assert.True(actual.HasErrors);
            Assert.AreEqual(1, actual.Errors.Count());
            Assert.AreEqual("Invalid Api end point action", actual.Errors.First().Subject);
            Assert.AreEqual("Api end point action is not mapped to actions in our system", actual.Errors.First().Message);
        }

        [Test]
        public void WHEN_all_input_are_empty_THEN_return_Error()
        {
            var actual = new InputValidator().Validate(
                "",
                "",
                ""
                );

            Assert.True(actual.HasErrors);
            Assert.AreEqual(3, actual.Errors.Count());
            Assert.AreEqual("Invalid Name", actual.Errors.First().Subject);
            Assert.AreEqual("Name is Empty", actual.Errors.First().Message);
        }
    }
}
