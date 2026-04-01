namespace Catalog.API.Products.GetProducts;

public record GetProductRequest(int? PageNumber =1, int? PageSize =10);
public record GetProductResponse(IEnumerable<Product> Products);

public class GetProductEndpoint : ICarterModule
{
    //Using CARTER to create the route
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductRequest getProductRequest, ISender sender) =>
        {
            var query = getProductRequest.Adapt<GetProductQuery>();
            var result = await sender.Send(query);
            // var result = await sender.Send(new GetProductQuery());
            var response = result.Adapt<GetProductResponse>();
            
            return Results.Ok(response);
        }).WithName("GetProducts")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products")
        .WithDescription("Get Products");
    }
}