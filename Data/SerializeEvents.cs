using System;
using Domain;
using Domain.ServiceAgent.Events;
using Newtonsoft.Json;

namespace Data
{
    public static class SerializeEvents
    {
        public static Result<string> SerializeEvent(Event @event)
        {
            string eventData;
            switch (@event)
            {
                case ServiceAgentScheduledEvent serviceAgentScheduledEvent:
                    eventData = JsonConvert.SerializeObject(serviceAgentScheduledEvent);
                    break;
                case ServiceAgentCompletedEvent serviceAgentCompletedEvent:
                    eventData = JsonConvert.SerializeObject(serviceAgentCompletedEvent);
                    break;
                case ServiceAgentFailedEvent serviceAgentFailedEvent:
                    eventData = JsonConvert.SerializeObject(serviceAgentFailedEvent);
                    break;
                default:
                    return Result.Failed<string>(
                        Error.CreateFrom("Serialize Event",
                            $"Not valid event type"));
            }
            return Result.Ok(eventData);
        }

        public static Event DeserializeEvent(
            string eventType,
            string eventData
        )
        {
            return eventType switch
            {
                nameof(ServiceAgentScheduledEvent)
                    => JsonConvert.DeserializeObject<ServiceAgentScheduledEvent>(eventData),
                nameof(ServiceAgentCompletedEvent)
                    => JsonConvert.DeserializeObject<ServiceAgentCompletedEvent>(eventData),
                nameof(ServiceAgentFailedEvent)
                    => JsonConvert.DeserializeObject<ServiceAgentFailedEvent>(eventData),

                _ => throw new AggregateException($"Couldn't process the event of Type {eventType}'")
            };
        }
    }
}
