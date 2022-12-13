using Core.Events;

namespace Account.Domain.Events
{
    public class AccountClosedEvent : BaseEvent
    {
        public AccountClosedEvent(Guid id) : base(id) { }
    }
}
