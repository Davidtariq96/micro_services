namespace Catalog.API.Products.CreateProduct;


public record CreateProductRequest(
    string Name,
    List<string> Category,
    decimal Price,
    string Description,
    string ImageFile);
public record CreateProductResponse(Guid Id);

//Using CARTER library to create minimal Endpoints for a simplify and efficient way of creating
//endpoints

public class CreateProductEndpoint : ICarterModule
{
    //An interface from ICarterModule
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products",
                async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();
                //Mediator to send the request through pipeline
                var result = await sender.Send(command);

                //Convert result type using Mapstar
                var response = result.Adapt<CreateProductResponse>();

                //Return the new product ID Using RESULT method from ASP.NET
                return Results.Created($"/products/{response.Id}", response);
            })
            .WithName("CreateProduct")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
    }
}