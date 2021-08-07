using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Domain.ServiceAgent;
using Domain.ServiceAgent.Aggregate;
using WebApi.dto;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/service-agent")]
    public class ServiceAgentController : ControllerBase
    {
        private readonly IServiceAgentWorkflow _serviceAgentWorkflow;
        private readonly IServiceAgentService _serviceAgentService; 

        public ServiceAgentController(
            IServiceAgentWorkflow serviceAgentWorkflow,
            IServiceAgentService serviceAgentService
        )
        {
            _serviceAgentWorkflow = serviceAgentWorkflow;
            _serviceAgentService = serviceAgentService; 
        }
           
        [HttpPost]
        [Route("schedule-service-agent")]
        public ActionResult ScheduleServiceAgent(
            [FromBody] ServiceAgentRequestDto serviceAgentRequestDto
        )
        {
            var serviceAgentResult =
                _serviceAgentWorkflow.Run(
                    serviceAgentRequestDto.Name,
                    serviceAgentRequestDto.ApiEndpoint,
                    serviceAgentRequestDto.ApiEndpointAction,
                    serviceAgentRequestDto.Headers.Select(t=> new Header(t.Key,t.Value)).ToList(),
                    serviceAgentRequestDto.Body.ToString(),
                    serviceAgentRequestDto.SubmittedDate
                );

            if (serviceAgentResult.HasErrors)
                return new BadRequestObjectResult(
                    serviceAgentResult.Errors
                        .Select(error => new
                        {
                            subject = error.Subject,
                            message = error.Message
                        }));

            var serviceAgentAggregate =
                _serviceAgentService
                    .Get(serviceAgentResult.Value.AggregateId);

            if (serviceAgentAggregate.HasErrors)
                return new BadRequestObjectResult(
                    serviceAgentAggregate.Errors
                        .Select(error => new
                        {
                            subject = error.Subject,
                            message = error.Message
                        }));

            return Ok(new
                {
                    ServiceAgentId = serviceAgentAggregate.Value.ServiceAgentId,
                    ResponseBody = serviceAgentAggregate.Value.ResponseBody,
                    ResponseHeaders = serviceAgentAggregate.Value.ResponseHeaders,
                    Status = serviceAgentAggregate.Value.Status.Id
            }
            );
        }
         
    }
}