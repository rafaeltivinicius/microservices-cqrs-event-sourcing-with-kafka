using CQRS.Core.Handlers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Domain.Handlers
{
    //public class NewPostCommandHandler : ICommandHandler<NewPostCommand>
    //{
    //    private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler;

    //    public NewPostCommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler)
    //    {
    //        _eventSourcingHandler = eventSourcingHandler;
    //    }

    //    public async Task HandleAsync(NewPostCommand command)
    //    {
    //        var aggregate = new PostAggregate();
    //        aggregate.CreatePost(command.Id, command.Author, command.Message);
    //        await _eventSourcingHandler.SaveAsync(aggregate);
    //    }
    //}
}
