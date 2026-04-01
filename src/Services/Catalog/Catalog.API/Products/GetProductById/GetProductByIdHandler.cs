
namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id): IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

internal class GetProductByIdQueryHandler (IDocumentSession session, ILogger<GetProductByIdQuery> logger)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting product by the {id}",query);
        var product = await session.LoadAsync<Product>(query.Id,cancellationToken);
        return product is null ? throw new ProductNotFoundException(query.Id) : new GetProductByIdResult(product);
    }
}