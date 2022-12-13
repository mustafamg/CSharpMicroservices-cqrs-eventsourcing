using Core.Exceptions;
using Core;
using Core.Events;

namespace Account.Domain
{
    public class AccountEventStore
    {
        IEventStoreRepository eventStoreRepository;
        public AccountEventStore(IEventStoreRepository eventStoreRepository)
        {
            this.eventStoreRepository = eventStoreRepository;
        }

        public void SaveEvents(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStream = eventStoreRepository.FindByAggregateIdentifier(aggregateId);
            if (expectedVersion != -1 && eventStream.Last().Version != expectedVersion)
            {
                throw new ConcurrencyException();
            }
            var version = expectedVersion;
            foreach (var evnt in events)
            {
                version++;
                evnt.Version = version;
                var eventModel = new EventModel
                {
                    TimeStamp = DateTime.Now,
                    AggregateIdentifier = aggregateId,
                    AggregateType = typeof(AccountAggregate).Name,
                    Version = version,
                    EventType = evnt.GetType().Name,
                    EventData = evnt
                };
                var persistedEvent = eventStoreRepository.Save(eventModel);

                if (persistedEvent != null)
                {
                    // TODO: produce event to event publisher if didn't use event store
                }
            }
        }
    }
}