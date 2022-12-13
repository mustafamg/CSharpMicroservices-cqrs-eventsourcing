using Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IEventStore
    {
        void SaveEvents(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
        List<BaseEvent> GetEvents(Guid aggregateId);
    }
}
