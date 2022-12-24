using EventStore.Client;

namespace Infrastructure
{
    public struct EventModel
    {
        public Uuid Id;
        public string EventType;
        public DateTime TimeStamp;
        public Guid AggregateIdentifier;
        public string AggregateType;
        public int Version;
        public ReadOnlyMemory<byte> EventData;
    }
}
