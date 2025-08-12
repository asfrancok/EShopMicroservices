namespace BuildingBlocks.Messaging.Events;

public record IntegrationEvent
{
    Guid Id => Guid.NewGuid();
    public DateTime OccuredOn => DateTime.UtcNow;
    public string EventTYpe => GetType().AssemblyQualifiedName;
}