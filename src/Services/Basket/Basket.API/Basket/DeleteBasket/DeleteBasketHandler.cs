namespace Basket.API.Basket.DeleteBasket;

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketResult>;

public record DeleteBasketResult(bool  IsSuccess);

//Validate items to be store in the constructor of this class

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotNull().WithMessage("Username is required");
    }
}

//Handler to perform the business logic
public class DeleteBasketCommandHandler (IBasketRepository basketRepository)
    : ICommandHandler<DeleteBasketCommand,DeleteBasketResult>
{
    public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
    {
        //Delete basket from DB and Cache
        await basketRepository.DeleteBasket(command.UserName,cancellationToken);
        return new DeleteBasketResult(true);
    }
}