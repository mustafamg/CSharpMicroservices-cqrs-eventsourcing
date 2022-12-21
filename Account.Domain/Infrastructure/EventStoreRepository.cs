using Core.Events;
using EventStore.Client;
using System;
using System.Linq;
using System.Text.Json;

namespace Account.Domain.Infrastructure
{
    public class EventStoreRepository
    {
        EventStoreClient _client;
        public EventStoreRepository()
        {
            var connection = EventStoreClientSettings.Create("esdb://127.0.0.1:2113?tls=false&keepAliveTimeout=10000&keepAliveInterval=10000");
            _client = new EventStoreClient(connection);
        }
        public async Task<int> GetAggregateLatestVersion(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            List<ResolvedEvent> rawEventStream =
                await GetAggregateStreamAsync(aggregateId, Direction.Backwards, cancellationToken);
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
            CancellationToken cancellationToken)
        {
            var result = _client.ReadStreamAsync(
                            direction,
                            aggregateId.ToString(),
                            StreamPosition.End,
                            cancellationToken: cancellationToken);
            if (await result.ReadState == ReadState.StreamNotFound)
            {
                return new List<ResolvedEvent>();
            }
            var rawEventStream = await result.ToListAsync(cancellationToken);
            return rawEventStream;
        }

        public async Task Save(Guid aggregateId,
            string aggregateName,
            BaseEvent @event,
            CancellationToken cancellationToken = default)
        {
            var eventData = new EventData
                (
                    Uuid.NewUuid(),
                    type: @event.GetType().Name,
                   data: JsonSerializer.SerializeToUtf8Bytes(@event),
                   metadata: JsonSerializer.SerializeToUtf8Bytes(new
                   {
                       @event.Version,
                       TimeStamp = DateTime.Now,
                       aggregateName
                   })
                );

            await _client.AppendToStreamAsync(
                    aggregateId.ToString(),
                    StreamState.Any,
                    new[] { eventData },
                    cancellationToken: cancellationToken
                );
        }
        public async Task<List<EventModel>> GetAllEvents(Guid aggregateId, CancellationToken cancellationToken = default)
        {
            var events = new List<EventModel>();
            List<ResolvedEvent> rawEventStream =
                await GetAggregateStreamAsync(aggregateId, Direction.Forwards, cancellationToken);
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
