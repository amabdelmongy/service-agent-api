using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Domain;
using Domain.ServiceAgent.Events;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace WebApi.Integration.Test.ServiceAgentControllerTests
{
    public class ServiceAgentControllerTests
    {
        private const string UrlScheduleServiceAgent = "/api/v1/service-agent/schedule-service-agent/";

        private HttpClient CreateClient(IEventRepository eventRepository)
        {
            var factory =
                new WebApplicationFactory<Startup>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureTestServices(services =>
                        {
                            services.AddScoped(a => eventRepository);
                        });
                    });

            return factory.CreateClient();
        }

        [Test]
        public async Task WHEN_Get_return_Exception_THEN_return_Error()
        {
            var expectedError =
                Error.CreateFrom(
                    "Failed calling Data base",
                    new Exception("Test Exception from Event Repository")
                );

            Result<IEnumerable<Event>> expectedResult = Result.Failed<IEnumerable<Event>>(expectedError);

            var eventRepository = new InMemoryEventRepository().WithNewGet(expectedResult);

            var client = CreateClient((IEventRepository)eventRepository);
            var response =
                await client.PostAsJsonAsync(
                    UrlScheduleServiceAgent,
                    StubDtoTests.ServiceAgentRequestDto
                );

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();

            var outputErrorDefinition = new[]
            {
                new
                {
                    subject = "",
                    message = ""
                }
            };

            var output = JsonConvert.DeserializeAnonymousType(result, outputErrorDefinition);
            Assert.AreEqual(expectedError.Subject, output[0].subject);
            Assert.AreEqual(expectedError.Message, output[0].message);
        }

        [Test]
        public async Task WHEN_Add_return_Exception_THEN_return_Error()
        {
            var expectedError = Error.CreateFrom(
                "Failed calling Data base",
                new Exception("Test Exception from Event Repository"));

            var expectedResult = Result.Failed<object>(expectedError);

            var eventRepository = new InMemoryEventRepository().WithNewAdd(expectedResult);

            var client = CreateClient((IEventRepository)eventRepository);
            var response =
                await client.PostAsJsonAsync(
                    UrlScheduleServiceAgent,
                    StubDtoTests.ServiceAgentRequestDto
                );

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync();

            var outputErrorDefinition = new[]
            {
                new
                {
                    subject = "",
                    message = ""
                }
            };

            var output = JsonConvert.DeserializeAnonymousType(result, outputErrorDefinition);
            Assert.AreEqual(expectedError.Subject, output[0].subject);
            Assert.AreEqual(expectedError.Message, output[0].message);
        }
    }
}