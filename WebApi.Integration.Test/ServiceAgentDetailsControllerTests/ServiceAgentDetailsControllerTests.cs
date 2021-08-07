using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Domain;
using Domain.ServiceAgent.Aggregate;
using Domain.ServiceAgent.Projection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;

namespace WebApi.Integration.Test.ServiceAgentDetailsControllerTests
{
    public class ServiceAgentDetailsControllerTests
    {
        private const string UrlServiceAgentGet = "/api/v1/service-agent-details/";

        private HttpClient CreateClient(List<ServiceAgentProjection> serviceAgentProjections)
        {
            var inMemoryProjectionRepository = new InMemoryProjectionRepository();
            serviceAgentProjections.ForEach(serviceAgentProjection =>
                inMemoryProjectionRepository.Add(serviceAgentProjection)
            );

            var factory =
                new WebApplicationFactory<Startup>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureTestServices(services =>
                        {
                            services.AddScoped(a =>
                                (IProjectionRepository) inMemoryProjectionRepository);
                        });
                    });

            return factory.CreateClient();
        }

        [Test]
        public async Task WHEN_Get_by_serviceAgent_id_THEN_return_correct_ServiceAgent()
        {
            var expectServiceAgentProjection = new ServiceAgentProjection
            { 
                ServiceAgentId = new Guid(),
                Name = StubDtoTests.Name,
                ApiEndpoint = StubDtoTests.ApiEndpoint,
                ApiEndpointAction = StubDtoTests.ApiEndpointActionString,
                Body = StubDtoTests.Body.ToString(),
                SubmittedDate = DateTime.Now,
                Status = "Requested",
                ExecutionDate = null,
                LastUpdatedDate = DateTime.Now,
            };

            var client = CreateClient(
                new List<ServiceAgentProjection>()
                {
                    expectServiceAgentProjection
                });

            var response =
                await client.GetAsync(
                    UrlServiceAgentGet + expectServiceAgentProjection.ServiceAgentId);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            var outputDefinition = new
            {
                Id = "",
                Name = "",
                Submitted = "",
                Execution = "",
                Status = "",
                ApiEndpoint = "",
                ApiEndpointAction = "",
                Body = "",
                Headers =
                    new[] {
                        new {
                            key = "",
                            value = ""
                        }
                    },
                LastUpdatedDate = "",
                ResponseBody = "",
                ResponseHeaders =
                    new[] {
                        new {
                            key = "",
                            value = ""
                        }
                    },
                FailedDetails = ""
            };
 
            var output = JsonConvert.DeserializeAnonymousType(result, outputDefinition);
            Assert.AreEqual(expectServiceAgentProjection.ServiceAgentId.ToString(), output.Id);
            Assert.AreEqual(expectServiceAgentProjection.Name, output.Name);
            Assert.AreEqual(expectServiceAgentProjection.ApiEndpoint, output.ApiEndpoint);
            Assert.AreEqual(expectServiceAgentProjection.ApiEndpointAction, output.ApiEndpointAction);
            Assert.AreEqual(expectServiceAgentProjection.Body, output.Body);
            Assert.AreEqual(expectServiceAgentProjection.Status, output.Status);
            Assert.AreEqual(expectServiceAgentProjection.ExecutionDate, output.Execution);
            Assert.AreEqual(expectServiceAgentProjection.LastUpdatedDate, DateTime.Parse(output.LastUpdatedDate));
        }

        [Test]
        public async Task WHEN_Repository_return_Exception_THEN_return_Error()
        {
            var expectedError = Error.CreateFrom(
                "Failed calling Data base",
                new Exception("Test Exception from ServiceAgentProjection Repository"));

            Result<ServiceAgentProjection> expectedResult = Result.Failed<ServiceAgentProjection>(expectedError);

            var inMemoryProjectionRepository = new InMemoryProjectionRepository();
            inMemoryProjectionRepository.WithNewGetResult(expectedResult);

            var factory =
                new WebApplicationFactory<Startup>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureTestServices(services =>
                        {
                            services.AddScoped(a =>
                                (IProjectionRepository) inMemoryProjectionRepository);
                        });
                    });

            var client = factory.CreateClient();

            var response =
                await client.GetAsync(
                    UrlServiceAgentGet + Guid.NewGuid());


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