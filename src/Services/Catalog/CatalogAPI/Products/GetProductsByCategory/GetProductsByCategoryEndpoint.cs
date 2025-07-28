using CatalogAPI.Models;
using CatalogAPI.Products.GetProducts;

namespace CatalogAPI.Products.GetProductsByCategory;

public record GetProductsByCategoryResponse(IEnumerable<Product> Products);

public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{Constants.ProductRootPath}/category/{{category}}", async (string category, ISender sender) =>
        {
            var result = await sender.Send(new GetProductsByCategoryQuery(category));
            return Results.Ok(result.Adapt<GetProductsByCategoryResponse>());
        })
        .WithName("GetProductByCategory")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Category")
        .WithDescription("Get product by category");
    }
}