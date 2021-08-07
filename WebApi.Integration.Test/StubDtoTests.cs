using System.Collections.Generic;
using WebApi.dto;

namespace WebApi.Integration.Test
{
    public static class StubDtoTests
    {
        public static readonly string Name = "Agent service Name";
        public static readonly object Body = "{ 'Body' : 'Body' }";
        public static readonly string ApiEndpoint = "https://github.com/";
        public static readonly string ApiEndpointActionString = "Get";
        public static readonly IList<HeaderDto> Headers = new List<HeaderDto> { new HeaderDto { Key = "key", Value = "value" } };

        public static readonly ServiceAgentRequestDto ServiceAgentRequestDto 
            = new ServiceAgentRequestDto{
                Name = Name,
                ApiEndpoint= ApiEndpoint,
                ApiEndpointAction = ApiEndpointActionString,
                Headers = Headers,
                Body = Body
            };
        }
    }
