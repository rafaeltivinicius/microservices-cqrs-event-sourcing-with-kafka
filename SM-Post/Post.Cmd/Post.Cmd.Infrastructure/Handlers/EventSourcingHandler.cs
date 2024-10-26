using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers
{
    public class EventSourcingHandler<T> : IEventSourcingHandler<T> where T : AggregateRoot, new()
    {
        private readonly IEventStore _eventStore;

        public EventSourcingHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            // Busca todos os eventos relacionados ao aggregateId no EventStore
            var events = await _eventStore.GetEventsAsync(id);
            if (events == null || events.Count == 0)
                throw new Exception("Aggregate not found.");

            // Instancia um novo agregado e aplica todos os eventos para reconstruir seu estado
            var aggregate = new T();
            aggregate.ReplayEvent(events);
            aggregate.Version = events.Select(x => x.Version).Max();

            return aggregate;
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventAsync(aggregate.Id, aggregate.GetUncommitredChanges(), aggregate.Version);
            aggregate.MarkChangesAsCommitted();
        }
    }
}