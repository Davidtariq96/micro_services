namespace Basket.API.Data;

public interface IBasketRepository
{
    //Define the 3 contracts with their parameters and response type on the
    //TASK<Response> that shares data i.e. GET,STORE & DELETE BASKET
    Task<ShoppingCart> GetBasket(string userName,CancellationToken cancellationToken = default);
    
    Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);
}