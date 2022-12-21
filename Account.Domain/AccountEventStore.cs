using Core.Exceptions;
using Core.Events;
using Core.Repositories;
using Cqrs.Core;
using Account.Domain.Infrastructure;
using Account.Domain.Events;
using System.Text.Json;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Account.Domain
{
    public class AccountEventStore : IEventStore
    {
        EventStoreRepository eventStoreRepository;
        public AccountEventStore(EventStoreRepository eventStoreRepository)
        {
            this.eventStoreRepository = eventStoreRepository;
        }

        public async Task SaveEvents(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion, CancellationToken cancellationToken = default)
        {
            string aggregateName = nameof(AccountAggregate);
            var version = await eventStoreRepository.GetAggregateLatestVersion(aggregateId,
                cancellationToken);

            if (expectedVersion != -1 && version != expectedVersion)
            {
                throw new ConcurrencyException();
            }

            foreach (var evnt in events)
            {
                version++;
                evnt.Version = version;

                //Saving to event store trigger the event, otherwise, we should push to event bus
                await eventStoreRepository.Save(aggregateId,
                    aggregateName, evnt, cancellationToken);
            }
        }

        async Task<BaseEvent> ParseEvent(EventModel eventModel)
        {
            switch (eventModel.EventType)
            {
                case nameof(AccountOpenedEvent):
                    return await JsonSerializer.DeserializeAsync<AccountOpenedEvent>(
                new MemoryStream(eventModel.EventData.ToArray()));
                case nameof(AccountClosedEvent):
                    return await JsonSerializer.DeserializeAsync<AccountClosedEvent>(
                new MemoryStream(eventModel.EventData?.ToArray()));
                case nameof(FundsDepositedEvent):
                    return await JsonSerializer.DeserializeAsync<FundsDepositedEvent>(
                new MemoryStream(eventModel.EventData.ToArray()));
                case nameof(FundsWithdrawnEvent):
                    return await JsonSerializer.DeserializeAsync<FundsWithdrawnEvent>(
                new MemoryStream(eventModel.EventData.ToArray()));
                default:
                    throw new ApplicationException("ParseEvent couldn't find the event to parse to");
            }
        }

        public async Task<List<BaseEvent>> GetEvents(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            List<BaseEvent> ret = new();
            List<EventModel> events = await eventStoreRepository.GetAllEvents(aggregateId);
            foreach (var @event in events)
            {
                ret.Add(await ParseEvent(@event));
            }
            return ret;
        }
    }
}