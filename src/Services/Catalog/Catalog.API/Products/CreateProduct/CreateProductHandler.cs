namespace Catalog.API.Products.CreateProduct;


public record CreateProductCommand(string Name, List<string>Category, decimal Price, string Description,string ImageFile)
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    //MEDIATOR PipelineBehaviour automatically invoke all these validation
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}


internal class CreateProductHandler (IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        
        
        //Create a Product Entity from command object
        var product = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Price = command.Price,
            Description = command.Description,
            ImageFile = command.ImageFile
        };
        //Save to database
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);
        //Return a CreateProductResult
      return new CreateProductResult(product.Id);
    }
}