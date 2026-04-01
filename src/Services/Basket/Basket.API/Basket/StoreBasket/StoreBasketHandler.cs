namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand (ShoppingCart Cart) : ICommand <StoreBasketResult>;

public record StoreBasketResult (string UserName);

//Validate items to be store in the constructor of this class

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can't be null");
        RuleFor(x => x.Cart.UserName).NotNull().WithMessage("Username is required");
    }
}

//Handler to perform the business logic of saving to DB
public class StoreBasketCommandHandler 
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        //get the cart object from the client side
        ShoppingCart cart = command.Cart;
        //Save to Database ( if existing UPDATE else INSERT )
        //Update Cache

        return new StoreBasketResult("Dummy name");
    }
}