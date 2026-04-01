namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool  IsSuccessful);

//Validate items to be store in the constructor of this class

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotNull().WithMessage("Username is required");
    }
}

//Handler to perform the business logic
public class DeleteBasketCommandHandler : ICommandHandler<DeleteBasketCommand,DeleteBasketResult>
{
    public Task<DeleteBasketResult> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        //Delete basket from DB and Cache
        return Task.FromResult(new DeleteBasketResult(true));
    }
}