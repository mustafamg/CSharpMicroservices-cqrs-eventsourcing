using System.Text.Json;
using Common.Infrastructure.Core;
using Common.Infrastructure.Core.BaseModels;
using EventStore.Client;

namespace Infrastructure
{
    public abstract class BaseEventStoreRepository : IEventStoreRepository
    {
        EventStoreClient _client;
        public BaseEventStoreRepository()
        {
            var connection = EventStoreClientSettings.Create("esdb://127.0.0.1:2113?tls=false&keepAliveTimeout=10000&keepAliveInterval=10000");
            _client = new EventStoreClient(connection);
        }
        public async Task<int> GetAggregateLatestVersion(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            List<ResolvedEvent> rawEventStream =
                await GetAggregateStreamAsync(aggregateId, Direction.Backwards, StreamPosition.End, cancellationToken);
            if (!rawEventStream.Any())
            {
                return 0;
            }

            var eventStream = await JsonSerializer.DeserializeAsync<BaseEvent>(
                new MemoryStream(rawEventStream.FirstOrDefault().Event.Data.ToArray()),
                cancellationToken: cancellationToken);
            return eventStream?.Version ?? -1;
        }

        private async Task<List<ResolvedEvent>> GetAggregateStreamAsync(
            Guid aggregateId,
            Direction direction,
            StreamPosition streamPosition,
            CancellationToken cancellationToken = default)
        {
            var result = _client.ReadStreamAsync(
                            direction,
                            aggregateId.ToString(),
                            streamPosition,
                            cancellationToken: cancellationToken);
            var results = await result.ReadState;
            if (results == ReadState.StreamNotFound)
            {
                return new List<ResolvedEvent>();
            }
            var rawEventStream = await result.ToListAsync(cancellationToken);
            return rawEventStream;
        }

        public async Task Save(Guid aggregateId,
            string aggregateName,
            IEnumerable<BaseEvent> events,
            CancellationToken cancellationToken = default)
        {
            var eventData = events.Select(ev => new EventData
                (
                   Uuid.NewUuid(),
                   type: ev.GetType().Name,
                   data: SerializeBasedOnEventType(ev),
                   metadata: JsonSerializer.SerializeToUtf8Bytes(new
                   {
                       ev.Version,
                       TimeStamp = DateTime.Now,
                       aggregateName
                   })
                ));

            await _client.AppendToStreamAsync(
                    aggregateId.ToString(),
                    StreamState.Any,
                    eventData,
                    cancellationToken: cancellationToken
                );
        }

        protected abstract byte[] SerializeBasedOnEventType(BaseEvent @event);

        public async Task<List<EventModel>> GetAllEvents(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var events = new List<EventModel>();
            List<ResolvedEvent> rawEventStream =
                await GetAggregateStreamAsync(aggregateId, Direction.Forwards, StreamPosition.Start, cancellationToken);
            if (!rawEventStream.Any())
            {
                return events;
            }

            foreach (var eventStream in rawEventStream)
            {
                using var stream = new MemoryStream(eventStream.Event.Data.ToArray());
                var eventModel = new EventModel()
                {
                    Id = eventStream.Event.EventId,
                    EventType = eventStream.Event.EventType,
                    TimeStamp = eventStream.Event.Created,
                    EventData = eventStream.Event.Data
                };

                events.Add(eventModel);
            }
            return events;
        }
    }
}
