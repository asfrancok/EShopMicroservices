namespace CatalogAPI.Products.DeleteProduct;

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{Constants.ProductRootPath}/{{id}}", async (Guid id, ISender sender) => 
        { 
            await sender.Send(new DeleteProductCommand(id));
            return Results.Ok();
        })
        .WithName("DeleteProductById")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Product By Id")
        .WithDescription("Delete product by id");
    }
}