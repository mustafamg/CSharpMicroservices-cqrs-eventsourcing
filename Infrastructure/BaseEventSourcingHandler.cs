﻿using Core.Repositories;
using Cqrs.Core;

namespace Core
{
    public abstract class BaseEventSourcingHandler<T> where T : AggregateRoot, new()
    {
        private readonly IEventStore eventStore;
        private T aggregate;

        public BaseEventSourcingHandler(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public async Task Save(T aggregate)
        {
            await eventStore.SaveEvents(
                aggregate.Id,
                aggregate.GetUncommittedChanges(),
                aggregate.Version);
            aggregate.CommitChanges();
        }

        public async Task<T> GetById(Guid id)
        {
            aggregate = new T();
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
