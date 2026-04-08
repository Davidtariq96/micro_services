
using Mapster;

namespace Basket.API.Basket.GetBasket;

// public record GetBasketRequest(string UserName);

public record GetBasketResponse(ShoppingCart Cart);

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //Send query-params via delegate method on the api url to handler using mediator
        app.MapGet("/basket/{userName}", async ( string userName ,ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery(userName));
            
            var response = result.Adapt<GetBasketResponse>();
            
            return Results.Ok(response);
        }).WithName("Get Basket")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Basket")
            .WithDescription("Get Basket");
    }
} 