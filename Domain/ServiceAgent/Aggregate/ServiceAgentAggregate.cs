using System;
using System.Collections.Generic;

namespace Domain.ServiceAgent.Aggregate
{
    public enum ApiEndpointAction {
        Get,
        Put,
        Post,
        Delete
    }
    public class Header {
        public string Key { get; }
        public string Value { get; }

        public Header(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
    public class ServiceAgentAggregate
    {
        public ServiceAgentAggregate()
        {

        }
        ServiceAgentAggregate(
            Guid id,
            int version,
            string name,
            string apiEndpoint,
            ApiEndpointAction apiEndpointAction,
            IList<Header> headers,
            string body,
            DateTime submittedDate,
            Status status,
            DateTime? executionDate = null,
            string responseBody = null,
            IList<Header> responseHeaders = null
        )
        {
            ServiceAgentId = id;
            Version = version;
            Name = name;
            ApiEndpoint = apiEndpoint;
            ApiEndpointAction = apiEndpointAction;
            Headers = headers;
            Body = body;
            SubmittedDate = submittedDate;
            Status = status;
            ExecutionDate = executionDate;
            ResponseHeaders = responseHeaders;
            ResponseBody = responseBody;
        }
        public Guid ServiceAgentId { get; }
        public string Name { get; }
        public string ApiEndpoint { get; }
        public ApiEndpointAction ApiEndpointAction { get; }
        public IList<Header> Headers { get; }
        public string Body { get; }
        public DateTime SubmittedDate{ get; }
        public Status Status { get; }
        public int Version { get; }
        public string ResponseBody { get; }
        public IList<Header> ResponseHeaders { get; }
        public DateTime? ExecutionDate { get; }

        public ServiceAgentAggregate With(
            Guid id,
            int version,
            string name,
            string apiEndpoint,
            ApiEndpointAction apiEndpointAction,
            IList<Header> headers,
            string body,
            DateTime submittedDate,
            Status status
        )
        {
            return
                new ServiceAgentAggregate(
                    id,
                    version,
                    name,
                    apiEndpoint,
                    apiEndpointAction,
                    headers,
                    body,
                    submittedDate,
                    status
                );
        }

        public ServiceAgentAggregate With(
            Status status,
            int version,
            DateTime executionDate,
            string responseBody = null,
            IList<Header> responseHeaders = null
        )
        {
            return
                new ServiceAgentAggregate(
                    ServiceAgentId,
                    version,
                    Name,
                    ApiEndpoint,
                    ApiEndpointAction,
                    Headers,
                    Body,
                    SubmittedDate,
                    status,
                    executionDate,
                    responseBody,
                    responseHeaders
                );
        }
    }
}