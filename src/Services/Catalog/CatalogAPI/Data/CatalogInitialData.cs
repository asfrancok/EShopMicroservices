using CatalogAPI.Models;
using Marten.Schema;

namespace CatalogAPI.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();
        
        if (await session.Query<Product>().AnyAsync())
            return;

        session.Store<Product>(CreatePreconfiguredProducts());
        await session.SaveChangesAsync();
    }

    private static IEnumerable<Product> CreatePreconfiguredProducts()
    {
        return new[]
        {
            new Product
            {
                Id = new Guid("1d089a32-f6ab-421d-bb0a-551658534dc3"),
                Name = "HTC U11+ Plus",
                Description = "This phone is company's biggest change.",
                ImageFile = "product-5.png",
                Price = 380.00M,
                Categories = new List<string> { "Smart Phone" }
            },
            new Product
            {
                Id = new Guid("7ec250e3-b644-4364-af75-4775e627fe3b"),
                Name = "LG G7 ThinQ",
                Description = "Newest top tier kitchen utensil",
                ImageFile = "product-6.png",
                Price = 240.00M,
                Categories = new List<string> { "Home Kitchen" }
            },
            new Product
            {
                Id = new Guid("eaca30fe-7eea-4c4c-9840-5c90fecb5d06"),
                Name = "Panasonic Lumix",
                Description = "Best camera",
                ImageFile = "product-7.png",
                Price = 239.00M,
                Categories = new List<string> { "Camera" }
            }
        };
    }
}