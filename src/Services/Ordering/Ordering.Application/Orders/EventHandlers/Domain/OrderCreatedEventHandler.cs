using MassTransit;
using Microsoft.FeatureManagement;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventHandler(IPublishEndpoint _publishEndpoint, IFeatureManager _featureManager, ILogger<OrderCreatedEventHandler> _logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event handled: {DomainEvent}", domainEvent.GetType().Name);

        if (await _featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            var integrationEvent = domainEvent.order.ToOrderDto();
            await _publishEndpoint.Publish(integrationEvent, cancellationToken);
        }
    }
}