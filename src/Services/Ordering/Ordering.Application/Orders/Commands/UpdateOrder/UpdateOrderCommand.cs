using BuildingBlocks.CQRS;
using FluentValidation;
using System.Windows.Input;

namespace Ordering.Application.Orders.Commands.UpdateOrder;

public record UpdateOrderCommand(OrderDto Order) : ICommand<UpdateOrderCommandResult>;

public record UpdateOrderCommandResult(bool IsSuccess);

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Order.OrderName).NotEmpty().WithName("Name is required");
        RuleFor(x => x.Order.CustomerId).NotNull().WithName("CustomerId is required");
    }
}