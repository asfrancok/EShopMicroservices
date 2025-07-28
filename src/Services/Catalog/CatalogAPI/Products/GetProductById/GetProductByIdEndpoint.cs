using CatalogAPI.Models;
using CatalogAPI.Products.GetProducts;

namespace CatalogAPI.Products.GetProductById;

public record GetProductByIdResponse(Product Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{Constants.ProductRootPath}/{{id}}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetProductByIdQuery(id));

            return Results.Ok(result.Adapt<GetProductByIdResponse>());
        })
        .WithName("GetProductById")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Id")
        .WithDescription("Get product by id");
    }
}
