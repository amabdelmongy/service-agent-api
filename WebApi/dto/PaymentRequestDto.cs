using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace WebApi.dto
{
    public class HeaderDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class ServiceAgentRequestDto
    {
        public string Name { get; set; }
        public string ApiEndpoint { get; set; }
        public string ApiEndpointAction { get; set; }
        public IList<HeaderDto> Headers { get; set; }
        public Object Body { get; set; }
        public string SubmittedDate { get; set; }
    }
    public class FavouriteDto
    {
        public Guid Id { get; set; }
        public bool IsFavourite { get; set; }
    }
}
