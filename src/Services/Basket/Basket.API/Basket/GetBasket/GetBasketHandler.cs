using Basket.API.Data;

namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

public record GetBasketResult (ShoppingCart Cart);

public class GetBasketQueryHandler (IBasketRepository basketRepository)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
       //Get basket from DATABASE via the repository
       var result = await basketRepository.GetBasket(query.UserName,cancellationToken);
       
       return new GetBasketResult(result);
    }
}