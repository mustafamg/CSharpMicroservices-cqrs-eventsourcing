using Common.Infrastructure.Core.BaseModels;

namespace Common.Infrastructure.Core
{
    public interface IEventStoreRepository
    {
        Task<int> GetAggregateLatestVersion(Guid aggregateId, CancellationToken cancellationToken = default);
        Task<List<EventModel>> GetAllEvents(Guid aggregateId, CancellationToken cancellationToken = default);
        Task Save(Guid aggregateId, string aggregateName, IEnumerable<BaseEvent> events, CancellationToken cancellationToken = default);
    }
}
