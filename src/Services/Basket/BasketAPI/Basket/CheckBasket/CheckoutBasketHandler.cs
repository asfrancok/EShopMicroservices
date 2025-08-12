
using BasketAPI.Data;
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace BasketAPI.Basket.CheckBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckout) : ICommand<CheckoutBasketResult>;

public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(c => c.BasketCheckout).NotNull().WithMessage("BasketCheckout can't be empty");
        RuleFor(c => c.BasketCheckout.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class CheckoutBasketCommandHandler(IBasketRepository _basketRepository, IPublishEndpoint _publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        var basket = await _basketRepository.GetBasket(command.BasketCheckout.UserName, cancellationToken);
        if (basket == null)
            return new CheckoutBasketResult(false);

        var eventMessage = command.BasketCheckout.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        await _publishEndpoint.Publish(eventMessage, cancellationToken);

        await _basketRepository.DeleteBasket(command.BasketCheckout.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}