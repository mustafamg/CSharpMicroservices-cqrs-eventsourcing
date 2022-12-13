using Core.Events;

namespace Account.Domain.Events
{
    public class FundsDepositedEvent:BaseEvent
    {
        public decimal Amount { get; set; }
        public FundsDepositedEvent(Guid id, decimal amount) :base(id)
        {
            Amount = amount;
        }
    }
    public class FundsWithdrawnEvent : BaseEvent
    {
        public decimal Amount { get; set; }
        public FundsWithdrawnEvent(Guid id, decimal amount) :base(id)
        {
            Amount = amount;
        }
    }public class AccountClosedEvent : BaseEvent
    {
        public AccountClosedEvent(Guid id) : base(id) { }
    }
}
