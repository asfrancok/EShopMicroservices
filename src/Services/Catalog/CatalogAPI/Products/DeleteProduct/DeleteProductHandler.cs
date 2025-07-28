using BuildingBlocks.CQRS;
using CatalogAPI.Models;

namespace CatalogAPI.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductCommandResult>;
public record DeleteProductCommandResult();

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty().WithMessage("Product ID is required");
    }
}

internal class DeleteProductCommandHandler(IDocumentSession _session) 
    : ICommandHandler<DeleteProductCommand, DeleteProductCommandResult>
{
    public async Task<DeleteProductCommandResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
         _session.Delete<Product>(command.Id);
        await _session.SaveChangesAsync(cancellationToken);

        return new DeleteProductCommandResult();
    }
}