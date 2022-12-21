using EventStore.Client;
using System;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Events
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
