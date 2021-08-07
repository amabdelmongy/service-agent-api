using Domain.ServiceAgent.Aggregate;
using System;
using System.Collections.Generic;

namespace Domain.ServiceAgent.InputValidator
{
    public class InputValidator : IInputValidator
    {
        public Result<object> Validate(
            string name,
            string apiEndpoint,
            string apiEndpointAction
        )
        {
            var errors = new List<Error>();
            errors.AddRange(ValidateName(name).Errors);
            errors.AddRange(ValidateApiEndpoint(apiEndpoint).Errors);
            errors.AddRange(ValidateEndpointAction(apiEndpointAction).Errors);
            return errors.Count > 0 ? Result.Failed<object>(errors) : Result.Ok<object>();
        }

        private Result<object> ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Result.Failed<object>(
                    Error.CreateFrom(
                        "Invalid Name",
                        "Name is Empty"
                    )
                );
            return Result.Ok<object>();
        }

        private Result<object> ValidateApiEndpoint(string apiEndpoint)
        {
            if (string.IsNullOrEmpty(apiEndpoint))
                return Result.Failed<object>(
                    Error.CreateFrom(
                        "Invalid Api end point",
                        "Api end point is Empty")
                );
            return Result.Ok<object>();
        }

        private Result<object> ValidateEndpointAction(string apiEndpointAction)
        {
            if (string.IsNullOrEmpty(apiEndpointAction))
                return Result.Failed<object>(
                    Error.CreateFrom(
                        "Invalid Api end point action",
                        "Api end point action is Empty")
                );

            ApiEndpointAction apiEndpointActionEnum;
            if (!Enum.TryParse<ApiEndpointAction>(apiEndpointAction, true, out apiEndpointActionEnum))
            {
                return Result.Failed<object>(
                    Error.CreateFrom(
                        "Invalid Api end point action",
                        "Api end point action is not mapped to actions in our system"
                    ));
            }
            return Result.Ok<object>();
        }
    }
}