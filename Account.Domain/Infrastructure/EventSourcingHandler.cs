using Account.Domain;
using Core.Repositories;
using Cqrs.Core;

namespace Core
{
    public class AccountEventSourcingHandler
    {
        private readonly IEventStore eventStore;

        public AccountEventSourcingHandler(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public async Task Save(AggregateRoot aggregate)
        {
            await eventStore.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
            aggregate.CommitChanges();
        }

        public async Task<AccountAggregate> GetById(Guid id)
        {
            var aggregate = new AccountAggregate();
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
