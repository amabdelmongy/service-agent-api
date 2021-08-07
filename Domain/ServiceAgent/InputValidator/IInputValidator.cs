namespace Domain.ServiceAgent.InputValidator
{
    public interface IInputValidator
    {
        Result<object> Validate(
            string name,
            string apiEndpoint,
            string apiEndpointAction
        );
    }
}