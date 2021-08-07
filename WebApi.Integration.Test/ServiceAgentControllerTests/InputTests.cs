using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Domain;
using Domain.ServiceAgent.Aggregate;
using Domain.ServiceAgentExecution;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using WebApi.Controllers.v1;
using WebApi.dto;

namespace WebApi.Integration.Test.ServiceAgentControllerTests
{
    public class InputTests
    {
        private readonly HttpClient _client;
        private const string UrlScheduleServiceAgent = "/api/v1/service-agent/schedule-service-agent/";

        public InputTests()
        {
            var executionFacadeMock = new Mock<IExecutionFacade>();

            executionFacadeMock
                .Setup(service =>
                    service.ProcessExecution(It.IsAny<ServiceAgentAggregate>()))
                .Returns(Result.Ok<ServiceAgentExecutionOutput>(
                    new ServiceAgentExecutionOutput { 
                        ExecutionDate = DateTime.Now,
                        ResponseBody = "ResponseBody",
                        ResponseHeaders = new List<Header>()
                    }));


            var factory =
                new WebApplicationFactory<Startup>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureTestServices(services =>
                        {
                            IEventRepository eventRepository = new InMemoryEventRepository(); 
                            services.AddScoped(a => eventRepository); 
                            services.AddScoped(a => executionFacadeMock.Object);
                        });
                    });

            _client = factory.CreateClient();
        }

        [Test]
        public async Task WHEN_ServiceAgentRequestDto_is_correct_THEN_return_OK()
        {

            var response =
                await _client.PostAsJsonAsync(
                    UrlScheduleServiceAgent,
                    StubDtoTests.ServiceAgentRequestDto);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var outputDefinition = new
            {
                serviceAgentId = "",
                responseBody = "",
                status = "",
                responseHeaders =
                    new[] {
                        new {
                                key = "",
                                value = ""
                        }
                    }
            };
            var output = JsonConvert.DeserializeAnonymousType(result, outputDefinition);
        }

        [Test]
        public async Task WHEN_ApiEndpointAction_empty_THEN_return_Error()
        {
            var serviceAgentRequestDto = new ServiceAgentRequestDto
                {
                    Name = StubDtoTests.Name,
                    ApiEndpoint = StubDtoTests.ApiEndpoint,
                    ApiEndpointAction = "",
                    Headers = StubDtoTests.Headers,
                    Body = StubDtoTests.Body
                };
            var response =
                await _client.PostAsJsonAsync(
                    UrlScheduleServiceAgent,
                    serviceAgentRequestDto
                );
             
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode );

            var result = await response.Content.ReadAsStringAsync();

            var outputErrorDefinition = new []
            {
                new
                {
                    subject = "",
                    message = ""
                }
            };

            var output = JsonConvert.DeserializeAnonymousType(result, outputErrorDefinition);
            Assert.AreEqual("Invalid Api end point action", output[0].subject);
            Assert.AreEqual("Api end point action is Empty", output[0].message);
        }

        [Test]
        public async Task WHEN_ApiEndpoint_empty_THEN_return_Error()
        {
            var serviceAgentRequestDto = new ServiceAgentRequestDto
            {
                Name = StubDtoTests.Name,
                ApiEndpoint = "",
                ApiEndpointAction = StubDtoTests.ApiEndpointActionString,
                Headers = StubDtoTests.Headers,
                Body = StubDtoTests.Body
            };
            var response =
                await _client.PostAsJsonAsync(
                    UrlScheduleServiceAgent,
                    serviceAgentRequestDto
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
            Assert.AreEqual("Invalid Api end point", output[0].subject);
            Assert.AreEqual("Api end point is Empty", output[0].message);
        }

        [Test]
        public async Task WHEN_name_empty_THEN_return_Error()
        {
            var serviceAgentRequestDto = new ServiceAgentRequestDto
            {
                Name = "",
                ApiEndpoint = StubDtoTests.ApiEndpoint,
                ApiEndpointAction = StubDtoTests.ApiEndpointActionString,
                Headers = StubDtoTests.Headers,
                Body = StubDtoTests.Body
            };
            var response =
                await _client.PostAsJsonAsync(
                    UrlScheduleServiceAgent,
                    serviceAgentRequestDto
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
            Assert.AreEqual("Invalid Name", output[0].subject);
            Assert.AreEqual("Name is Empty", output[0].message);
        }
    }
}