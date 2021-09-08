namespace Communication.Messages
{
    public interface IScheduledDistributedCommand : IDistributedCommand
    {
        bool IsDiffered { get; init; }
        void Differ();
    }
}
