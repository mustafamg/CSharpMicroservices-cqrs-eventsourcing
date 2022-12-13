using System;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Events
{
    public struct EventModel
    {
        public string Id;
        public DateTime TimeStamp;
        public Guid AggregateIdentifier;
        public string AggregateType;
        public int Version;
        public string EventType;
        public BaseEvent EventData;
    }
}
