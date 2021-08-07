using Domain.ServiceAgent.Aggregate;
using System;
using System.Collections.Generic;

namespace Domain.ServiceAgent.Projection
{
    public class ServiceAgentProjection
    {
        public Guid ServiceAgentId { get; set; }
        public string Name { get; set; }
        public string ApiEndpoint { get; set; }
        public String ApiEndpointAction { get; set; }
        public String Headers { get; set; }
        public string Body { get; set; }
        public DateTime SubmittedDate { get; set; }
        public String Status { get; set; }
        public string ResponseBody { get; set; }
        public string ResponseHeaders { get; set; }
        public DateTime? ExecutionDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string FailedDetails { get; set; }
        public bool? IsFavourite { get; set; }
    }
}

