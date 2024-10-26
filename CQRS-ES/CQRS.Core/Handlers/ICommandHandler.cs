namespace CQRS.Core.Handlers
{
    public interface ICommandHandler<TCommand> where TCommand : class
    {
        Task HandleAsync(TCommand command);
    }
}
