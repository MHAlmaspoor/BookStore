namespace BookStore.BuildingBlocks.Messaging;

public abstract record IntegrationEvent
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public virtual string EventType => GetType().Name;
    public virtual int Version => 1;
    public abstract string RoutingKey { get; }
}
