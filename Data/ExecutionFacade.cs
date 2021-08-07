using System;
using Domain;
using Domain.ServiceAgentExecution;
using Domain.ServiceAgent.Aggregate;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace Data
{
    public class ExecutionFacade : IExecutionFacade
    {
        private Result<ServiceAgentExecutionOutput> Execute(
            string URL,
            Guid serviceAgentId,
            IList<Header> inputHeaders,
            string body,
            ApiEndpointAction apiEndpointAction
        )
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var inputHeader in inputHeaders)
            {
                client.DefaultRequestHeaders.Add(inputHeader.Key, inputHeader.Value);
            }

            HttpResponseMessage response = ApplyAction(URL, body, apiEndpointAction, client);

            client.Dispose();
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var headersResult = new List<Header>();
                foreach (var header in response.Content.Headers)
                {
                    if (header.Value != null)
                        headersResult.Add(new Header(header.Key, string.Join(",", header.Value)));
                }
                return Result.Ok(new ServiceAgentExecutionOutput
                {
                    ExecutionDate = DateTime.Now,
                    ResponseBody = result,
                    ResponseHeaders = headersResult
                });
            }
            else
            {
                return Result.Failed<ServiceAgentExecutionOutput>(
                    RejectedServiceAgentExecutionError.CreateFrom(
                        serviceAgentId,
                        $"Execution to service failed with id {serviceAgentId}, " +
                        $"StatusCode: { response.StatusCode } " +
                        $"ReasonPhrase { response.ReasonPhrase }"
                    )
                );
            }
        }

        private static HttpResponseMessage ApplyAction(string URL, string body, ApiEndpointAction apiEndpointAction, HttpClient client)
        {
            HttpResponseMessage response;
            switch (apiEndpointAction)
            {
                case ApiEndpointAction.Get:
                    response = client.GetAsync(URL).Result;
                    break;
                case ApiEndpointAction.Post:
                    response = client.PostAsJsonAsync(URL, JsonConvert.DeserializeObject(body)).Result;
                    break;
                case ApiEndpointAction.Put:
                    response = client.PutAsJsonAsync(URL, JsonConvert.DeserializeObject(body)).Result;
                    break;
                case ApiEndpointAction.Delete:
                    response = client.DeleteAsync(URL).Result;
                    break;
                default:
                    throw new NotImplementedException($"ApiEndpointAction { apiEndpointAction } is not defined");
            }

            return response;
        }

        public Result<ServiceAgentExecutionOutput> ProcessExecution(ServiceAgentAggregate serviceAgentAggregate)
        {
            try
            {
                return Execute(
                    serviceAgentAggregate.ApiEndpoint,
                    serviceAgentAggregate.ServiceAgentId,
                    serviceAgentAggregate.Headers,
                    serviceAgentAggregate.Body,
                    serviceAgentAggregate.ApiEndpointAction
                    );
            }
            catch (Exception e)
            {
                return
                    Result.Failed<ServiceAgentExecutionOutput>(
                        Error.CreateFrom($"Failed process service agent execution ServiceAgentId :{ serviceAgentAggregate.ServiceAgentId }", e)
                    );
            }
        }
    }
}