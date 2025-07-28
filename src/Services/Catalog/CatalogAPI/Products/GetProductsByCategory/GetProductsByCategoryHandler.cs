using BuildingBlocks.CQRS;
using CatalogAPI.Models;

namespace CatalogAPI.Products.GetProductsByCategory;

public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductsByCategoryResult>;
public record GetProductsByCategoryResult(IEnumerable<Product> Products);

internal class GetProductsByCategoryHandler(IDocumentSession _session)
    : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
{
    public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {      
        var products  = await _session.Query<Product>()
            .Where(p => p.Categories.Contains(query.Category))
            .ToListAsync();

        return new GetProductsByCategoryResult(products);
    }
}