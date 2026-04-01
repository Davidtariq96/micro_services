using MediatR;

namespace BuildingBlocks.CQRS;

//IF it doesn't returns a response use this
public interface ICommandHandler<in TCommand >
    : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand<Unit>
   
{
    
}
// IF it returns a RESPONSE USE THIS
public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, TResponse>
   where TCommand :ICommand<TResponse>
   where TResponse: notnull
{
    
}