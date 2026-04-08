using Discount.Grpc;

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
public class StoreBasketCommandHandler (IBasketRepository basketRepository,
    DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        //Communicate with Discount.GRPC to get any available discount prices
        //before storing the basket
        //get the cart object from the client side
        await DeductDiscount(command.Cart,cancellationToken);
        ShoppingCart cart = command.Cart;
        //Save to Database ( if existing UPDATE else INSERT )
        var result = await basketRepository.StoreBasket(cart,cancellationToken);
        //Update Cache

        return new StoreBasketResult(cart.UserName);
    }

    public async Task DeductDiscount(ShoppingCart cart,CancellationToken cancellationToken )
    {
        foreach (var item in cart.Items)
        {
            var coupon =await discountProto.GetDiscountAsync(
                new GetDiscountRequest{ProductName =  item.ProductName},cancellationToken:cancellationToken);

            item.Price -= coupon.Amount;
        }
    }
}