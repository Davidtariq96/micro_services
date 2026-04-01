using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviours;

public class ValidationBehaviours <TRequest, TResponse>
    //I used IEnumerable To accomodate all Handle methods
    (IEnumerable<IValidator <TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults =
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        
        var failures = 
            validationResults.Where(r=>r.Errors.Any())
                .SelectMany(c => c.Errors)
                .ToList();
        
        if(failures.Any())
        {
            throw new ValidationException(failures);
        }
        //Move to the next action in the request PIPELINE After validation
        return await next(cancellationToken);
    }
}