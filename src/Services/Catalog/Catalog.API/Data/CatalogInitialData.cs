using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
         var session =  store.LightweightSession();
         if (await session.Query<Product>().AnyAsync(cancellation))
             return;
         //MARTEN UPSET caters for existing data
         session.Store<Product>(GetPreconfiguredProducts());
         await session.SaveChangesAsync(cancellation);
         
    }
    
    private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>
{
    new Product
    {
        Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
        Name = "iPhone X",
        Description = "Apple's revolutionary smartphone with Face ID and edge-to-edge display.",
        ImageFile = "product-1.png",
        Price = 950.00M,
        Category = new List<string> { "Smart Phone" }
    },
    new Product
    {
        Id = new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
        Name = "Samsung Galaxy S10",
        Description = "Samsung flagship phone with AMOLED display and powerful performance.",
        ImageFile = "product-2.png",
        Price = 840.00M,
        Category = new List<string> { "Smart Phone" }
    },
    new Product
    {
        Id = new Guid("9f1c2e3d-7a6b-4d1e-8f2a-123456789abc"),
        Name = "Google Pixel 6",
        Description = "Google phone with excellent camera and pure Android experience.",
        ImageFile = "product-3.png",
        Price = 780.00M,
        Category = new List<string> { "Smart Phone" }
    },
    new Product
    {
        Id = new Guid("1a2b3c4d-5e6f-7a8b-9c0d-abcdef123456"),
        Name = "MacBook Pro M2",
        Description = "High-performance laptop powered by Apple M2 chip.",
        ImageFile = "product-4.png",
        Price = 1800.00M,
        Category = new List<string> { "Laptop" }
    },
    new Product
    {
        Id = new Guid("abcdef12-3456-7890-abcd-ef1234567890"),
        Name = "Dell XPS 13",
        Description = "Compact and powerful ultrabook with premium design.",
        ImageFile = "product-5.png",
        Price = 1500.00M,
        Category = new List<string> { "Laptop" }
    },
    new Product
    {
        Id = new Guid("11223344-5566-7788-99aa-bbccddeeff00"),
        Name = "Sony WH-1000XM5",
        Description = "Industry-leading noise cancelling wireless headphones.",
        ImageFile = "product-6.png",
        Price = 400.00M,
        Category = new List<string> { "Audio" }
    }
};
}