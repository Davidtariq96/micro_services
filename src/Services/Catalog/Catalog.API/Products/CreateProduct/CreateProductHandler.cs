using MediatR;

namespace Catalog.API.Products.CreateProduct;
public record CreateProductCommand(string Name, List<string>Category, decimal Price, string Description)
    : IRequest<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductHandler: IRequestHandler<CreateProductCommand, CreateProductResult>
{
    public Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        //Business logic to create new product
        throw new NotImplementedException();
    }
}