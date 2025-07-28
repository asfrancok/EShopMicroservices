namespace CatalogAPI.Products.CreateProduct;

public record CreateProductRequest(string Name, List<string> Categories, string Description, string ImageFile, decimal Price);
public record CreateProductResponse(Guid Id);


public class CreateProductEndpoint : ICarterModule
{
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(Constants.ProductRootPath, async (CreateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateProductResponse>();
            return Results.Created($"{Constants.ProductRootPath}/{response.Id}", response);
        })
        .WithName("CreateProduct")
        .Produces<CreateProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Product")
        .WithDescription("Create Product");
    }
} 