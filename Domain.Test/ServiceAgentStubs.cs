using System;
using System.Collections.Generic;
using Domain.ServiceAgent.Aggregate;
using Domain.ServiceAgent.Commands;
using Domain.ServiceAgent.Events;

namespace Domain.Test
{
    static class ServiceAgentStubsTests
    {
        public static readonly Guid ServiceAgentIdTest = Guid.Parse("977d39b4-2a34-4223-baac-8496ff2a7bc1");
        public static readonly string Name = "Agent service Name";
        public static readonly string Body = "{ 'Body' : 'Body' }";
        public static readonly string ApiEndpoint = "https://github.com/";
        public static readonly string ApiEndpointActionString = "Get";
        public static readonly IList<Header> Headers = new List<Header> { new Header("key","value") };
        public static readonly DateTime SubmittedDate = new DateTime(2025,1,1);

        public static ServiceAgentAggregate ServiceAgentAggregateTest()
        {
            var serviceAgentAggregate = AggregateFactory.CreateFrom(
                new List<Event>
                {
                    new ServiceAgentScheduledEvent(
                        ServiceAgentIdTest,
                        DateTime.Now,
                        1,
                        "Name",
                        "ApiEndpoint",
                        ApiEndpointAction.Get,
                        new List<Header>(),
                        "Body",
                        DateTime.Now,
                        Status.Scheduled
                    )
                }
            );
            return serviceAgentAggregate.Value;
        } 

        public static readonly ServiceAgentScheduledEvent ServiceAgentScheduledEventTest =
            new ServiceAgentScheduledEvent(
                ServiceAgentIdTest,
                DateTime.Now,
                1,
                Name,
                ApiEndpoint,
                ApiEndpointAction.Get,
                Headers,
                Body,
                DateTime.Now,
                Status.Scheduled
            );

        public static readonly ScheduleServiceAgentCommand scheduleServiceAgentCommand =
            new ScheduleServiceAgentCommand(
                Name,
                ApiEndpoint,
                ApiEndpointAction.Get,
                Headers,
                Body,
                Status.Scheduled,
                SubmittedDate
            );
    }
}
