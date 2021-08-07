using System;
using System.Collections.Generic;
using Domain.ServiceAgent.Aggregate;

namespace Domain.ServiceAgentExecution
{
    public class ServiceAgentExecutionOutput {
        public string ResponseBody { get; set; }
        public IList<Header> ResponseHeaders { get; set; }
        public DateTime ExecutionDate { get; set; }
    }
    public interface IExecutionFacade
    {
        Result<ServiceAgentExecutionOutput> ProcessExecution(ServiceAgentAggregate paymentAggregate);
    }
}
