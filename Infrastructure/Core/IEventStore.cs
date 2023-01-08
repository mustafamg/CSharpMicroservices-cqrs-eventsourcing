using Common.Infrastructure.Core.BaseModels;

namespace Core.Repositories
{
    public interface IEventStore
    {
        Task SaveEvents(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion, CancellationToken cancellationToken = default);
        Task<List<BaseEvent>> GetEvents(Guid aggregateId, CancellationToken cancellationToken = default);
    }
}
