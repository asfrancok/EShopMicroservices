using BuildingBlocks.Messaging.Events;
using MassTransit;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class BasketCheckoutEventHandler(ISender _sender, ILogger<BasketCheckoutEventHandler> _logger)
    : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        _logger.LogInformation("Integration event handled: {IntegrationEvent}", context.Message.GetType().Name);

        var command = MapToCreateOrderCommand(context.Message);
        await _sender.Send(command);
    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine, message.Country, message.State, message.ZipCode);
        var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV, message.PaymentMethod);
        Guid orderId = Guid.NewGuid();

        var orderDto = new OrderDto(
            orderId,
            message.CustomerId,
            message.UserName,
            addressDto,
            addressDto,
            paymentDto,
            Ordering.Domain.Enums.OrderStatus.Pending,
            [
                new OrderItemDto(orderId, new Guid("e0cfa8d0-0da6-4c47-9b29-c687b10394fa"), 2, 500),
                new OrderItemDto(orderId, new Guid("86eca081-bfcc-4ae8-996a-1ee9b724e875"), 1, 4000)
            ]);

        return new CreateOrderCommand(orderDto);
    }
}