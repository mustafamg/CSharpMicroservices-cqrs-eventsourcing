using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Events;

namespace Core
{
    public interface IEventStoreRepository
    {
        List<EventModel> FindByAggregateIdentifier(Guid aggregateId);
        //TODO: update to fit with event store db used
        object Save(EventModel eventModel);
    }
}
