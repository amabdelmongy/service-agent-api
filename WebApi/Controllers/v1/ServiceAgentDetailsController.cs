using System;
using System.Collections.Generic;
using System.Linq;
using Domain.ServiceAgent.Projection;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.dto;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v1/service-agent-details")]
    public class ServiceAgentDetailsController : ControllerBase
    { 
        private readonly IProjectionRepository _projectionRepository;

        public ServiceAgentDetailsController( 
            IProjectionRepository projectionRepository
        )
        {
            _projectionRepository = projectionRepository;
        }

        [HttpGet]

        //[Route("?isShowFavourite={isShowFavouriteOnly}")]
        public ActionResult Get(bool? showFavourite)
        {
            var serviceAgentResult =
                _projectionRepository.Get(showFavourite);

            if (serviceAgentResult.HasErrors)
                return new BadRequestObjectResult(
                    serviceAgentResult.Errors
                        .Select(error => new
                        {
                            subject = error.Subject,
                            message = error.Message
                        }));

            if (serviceAgentResult.Value == null)
                return new NotFoundResult();

            var serviceAgents = serviceAgentResult.Value;

            return Ok(
                serviceAgents.ToList().Select(serviceAgent =>
                    new
                    {
                        Id = serviceAgent.ServiceAgentId,
                        Name = serviceAgent.Name,
                        Submitted = serviceAgent.SubmittedDate,
                        Execution = serviceAgent.ExecutionDate,
                        Status = serviceAgent.Status,
                        IsFavourite = serviceAgent.IsFavourite
                    })
                );
        }

        [HttpGet]
        [Route("{serviceAgentId}")]
        public ActionResult Get(Guid serviceAgentId)
        {
            var serviceAgentResult =
                _projectionRepository.Get(serviceAgentId);

            if (serviceAgentResult.HasErrors)
                return new BadRequestObjectResult(
                    serviceAgentResult.Errors
                        .Select(error => new
                        {
                            subject = error.Subject,
                            message = error.Message
                        }));

            if (serviceAgentResult.Value == null)
                return new NotFoundResult();

            var serviceAgent = serviceAgentResult.Value;
            var headerDtoDefinition = new[]
            {
                new
                {
                    key = "",
                    value = ""
                }
            };

            return Ok(
                new
                {
                    Id = serviceAgent.ServiceAgentId,
                    Name = serviceAgent.Name,
                    Submitted = serviceAgent.SubmittedDate,
                    Execution = serviceAgent.ExecutionDate,
                    Status = serviceAgent.Status,
                    ApiEndpoint = serviceAgent.ApiEndpoint,
                    ApiEndpointAction = serviceAgent.ApiEndpointAction,
                    Body = serviceAgent.Body,
                    Headers = serviceAgent.Headers == null? null : JsonConvert.DeserializeAnonymousType(serviceAgent.Headers, headerDtoDefinition),
                    LastUpdatedDate = serviceAgent.LastUpdatedDate,
                    ResponseBody = serviceAgent.ResponseBody,
                    ResponseHeaders = serviceAgent.ResponseHeaders == null ? null : JsonConvert.DeserializeAnonymousType(serviceAgent.ResponseHeaders, headerDtoDefinition),
                    FailedDetails = serviceAgent.FailedDetails
                }
            );
        }


        [HttpPut]
        [Route("update-is-favourite")]
        public ActionResult UpdateIsFavourite(
            [FromBody] FavouriteDto favouriteDto)
        {
            var serviceAgentResult =
                _projectionRepository.UpdateIsFavourite(
                    favouriteDto.Id,
                    favouriteDto.IsFavourite);

            if (serviceAgentResult.HasErrors)
                return new BadRequestObjectResult(
                    serviceAgentResult.Errors
                        .Select(error => new
                        {
                            subject = error.Subject,
                            message = error.Message
                        }));

            return Ok( 
            );
        }
    }
}