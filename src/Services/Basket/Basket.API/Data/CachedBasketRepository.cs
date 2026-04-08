namespace Basket.API.Data;

//Inject IBasketRepository repo in other to be able to 
//implement its methods i.e. CachedBasketRepository is acting as a PROXY pattern
//extending the via DECORATIVE PATTERN by adding Caching

public class CachedBasketRepository (IBasketRepository basketRepository, IDistributedCache distributedCache)
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await distributedCache.GetStringAsync(userName, cancellationToken);
        //if username it's not null serialize the value attached to it and return the data
        if (!string.IsNullOrEmpty(cachedBasket))
        {
            try
            {
                var shoppingCart = JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);
                if (shoppingCart != null)
                {
                    return shoppingCart;   
                }
            }
            catch (Exception ex)
            {
                // Log the deserialization error 
            }
        }
        
        var basket = await basketRepository.GetBasket(userName, cancellationToken);
        await distributedCache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }
    

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
         await basketRepository.StoreBasket(basket, cancellationToken);
         
         await distributedCache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
         return basket;
    }
    

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
         await basketRepository.DeleteBasket(userName, cancellationToken);
         
         await  distributedCache.RemoveAsync(userName,cancellationToken);
         return true;
    }
}