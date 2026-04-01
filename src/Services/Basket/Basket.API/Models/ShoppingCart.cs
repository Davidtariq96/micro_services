namespace Basket.API.Models;

public class ShoppingCart
{
    public string UserName { get; set; } = default!;
    
    public List<ShoppingCartItem> Items { get; set; } = new ();

    public decimal TotalPrice => Items.Sum( c => c.Price * c.Quantity );

    public ShoppingCart(string userName)
    {
        UserName = userName;
    }
    
    //For Mapping
    public ShoppingCart()
    {
    }
    
}