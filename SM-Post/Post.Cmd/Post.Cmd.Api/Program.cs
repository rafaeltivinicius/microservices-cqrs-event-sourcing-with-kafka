using CQRS.Core.Domain;
using CQRS.Core.Infrastructure;
using Post.Cmd.Api.CommandHandlers;
using Post.Cmd.Api.Commands;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Handlers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;
using Post.Cmd.Domain.Aggregates;
using CQRS.Core.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection("MongoDbConfig"));

// Registrar IEventStore e Event Store Repository
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();

// Registrar IEventSourcingHandler com sua implementação para PostAggregate
builder.Services.AddTransient<IEventSourcingHandler<PostAggregate>, EventSourcingHandler<PostAggregate>>();

// Registrar o Command Handler
builder.Services.AddScoped<NewPostCommandHandler>();

// Registrar o Command Dispatcher como Singleton
//builder.Services.AddSingleton<ICommandDispatcher, CommandDispatcher>();



// Adicionar handlers específicos para cada comando usando uma chamada pós-configuração do CommandDispatcher
builder.Services.AddScoped<ICommandDispatcher>(serviceProvider =>
{
    var dispatcher = new CommandDispatcher();

    // Registrar o handler para NewPostCommand no dispatcher
    dispatcher.RegisterHandler<NewPostCommand>(command =>
    {
        var handler = serviceProvider.GetRequiredService<NewPostCommandHandler>();
        return handler.HandleAsync(command);
    });

    return dispatcher;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
