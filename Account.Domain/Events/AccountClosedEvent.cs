using Common.Infrastructure.Core.BaseModels;

namespace Account.Domain.Events
{
    public class AccountClosedEvent : BaseEvent
    {
        public AccountClosedEvent(Guid id) : base(id) { }
    }
}
