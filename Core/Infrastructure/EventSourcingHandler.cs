using Core.Repositories;
using Cqrs.Core;

namespace Core
{
    public class EventSourcingHandler<T> where T : AggregateRoot, new()
    {
        private readonly IEventStore eventStore;

        public EventSourcingHandler(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public async Task Save(AggregateRoot aggregate)
        {
            await eventStore.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
            aggregate.CommitChanges();
        }

        public async Task<T> GetById(Guid id)
        {
            var aggregate = new T();
            var events = await eventStore.GetEvents(id);
            if (events != null && events.Any())
            {
                aggregate.ReplayEvents(events);
                var latestVersion = events.Max(e => e.Version);
                aggregate.Version = latestVersion;
            }
            return aggregate;
        }
    }
}
