namespace CatalogAPI.Products.UpdateProduct;

public record UpdateProductRequest(Guid Id, string Name, List<string> Categories, string Description, string ImageFile, decimal Price);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut($"{Constants.ProductRootPath}", async (UpdateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateProductCommand>();
            await sender.Send(command);

            return Results.Ok();
        })
        .WithName("UpdateProduct")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update product by ID")
        .WithDescription("Update product by ID");
    }
}