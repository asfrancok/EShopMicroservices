using BasketAPI.Data;
using static DiscountGrpc.DiscountProtoService;

namespace BasketAPI.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null").SetValidator(new ShoppingCartValidator());
    }
}

public class ShoppingCartValidator : AbstractValidator<ShoppingCart>
{
    public ShoppingCartValidator()
    {
        RuleFor(c => c.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class StoreBasketCommandHandler(IBasketRepository _repository, DiscountProtoServiceClient _discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        foreach (var item in command.Cart.Items)
        {
            var coupon = await _discountProto.GetDiscountAsync(new DiscountGrpc.GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
            item.Price -= coupon.Amount;
        }

        await _repository.StoreBasket(command.Cart, cancellationToken);       
        return new StoreBasketResult(command.Cart.UserName);
    }
}