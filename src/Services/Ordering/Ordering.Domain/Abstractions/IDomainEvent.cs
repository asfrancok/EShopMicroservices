using MediatR;

namespace Ordering.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    Guid EventID => Guid.NewGuid();
    DateTime OccuredOn => DateTime.UtcNow;
    string EventType => GetType().AssemblyQualifiedName!;
}