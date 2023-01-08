using Common.Infrastructure.Core.BaseModels;
using Core.Exceptions;
using Core.Repositories;

namespace Infrastructure
{
    //TODO: for test automation
    // https://github.com/linedata/reactive-domain/blob/33d04df1b959fbab9e01af47d5f22a5d9d1d374a/src/ReactiveDomain.Testing/EmbeddedEventStoreFixture.cs
    public abstract class BaseEventStore : IEventStore
    {
        private readonly BaseEventStoreRepository eventStoreRepository;
        private readonly string _aggregateName;
        public BaseEventStore(BaseEventStoreRepository eventStoreRepository, string aggregateName)
        {
            _aggregateName = aggregateName;
            this.eventStoreRepository = eventStoreRepository;
        }

        public async Task SaveEvents(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion, CancellationToken cancellationToken = default)
        {
            var version = await eventStoreRepository.GetAggregateLatestVersion(aggregateId,
                cancellationToken);

            if (expectedVersion != -1 && version != expectedVersion)
            {
                throw new ConcurrencyException();
            }

            //TODO: remove the for loop and save all events at once as a one transaction
            foreach (var evnt in events)
            {
                version++;
                evnt.Version = version;
            } 
            //Saving to event store trigger the event, otherwise, we should push to event bus
            await eventStoreRepository.Save(aggregateId,
                    _aggregateName, events, cancellationToken);
        }

        protected abstract Task<BaseEvent?> ParseEvent(EventModel eventModel);

        public async Task<List<BaseEvent>> GetEvents(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            List<BaseEvent> ret = new();
            List<EventModel> events = await eventStoreRepository.GetAllEvents(aggregateId);
            foreach (var @event in events)
            {
                var parsedEvent = await ParseEvent(@event);
                if (parsedEvent != null)
                    ret.Add(parsedEvent);
                else
                    throw new ApplicationException($"Event parsing failed and returned null:{@event.GetType().Name}");
            }
            return ret;
        }
    }
}