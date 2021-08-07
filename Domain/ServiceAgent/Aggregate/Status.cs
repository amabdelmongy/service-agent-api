namespace Domain.ServiceAgent.Aggregate
{
    public class Status
    {
        public static readonly Status Scheduled =
            new Status("Scheduled");

        public static readonly Status Completed =
            new Status("Completed");

        public static readonly Status Failed =
            new Status("Failed");

        public Status(string id)
        {
            Id = id;
        }
        public string Id { get; }
    }
}
