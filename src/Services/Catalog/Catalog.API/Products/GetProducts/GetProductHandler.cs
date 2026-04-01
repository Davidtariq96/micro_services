namespace Catalog.API.Products.GetProducts;


public record GetProductQuery(int? PageNumber =1, int? PageSize =10):IQuery<GetProductResult>;

public record GetProductResult(IEnumerable<Product> Products);

internal class GetProductQueryHandler (IDocumentSession session): IQueryHandler<GetProductQuery, GetProductResult>
{
    //Interface from IQueryHandler
    public async Task<GetProductResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>().ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10,cancellationToken);
            // .ToListAsync(cancellationToken);
        return new GetProductResult(products);
    }
}