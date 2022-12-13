using Core.Events;

namespace Account.Domain.Events
{
    public class FundsWithdrawnEvent : BaseEvent
    {
        public decimal Amount { get; set; }
        public FundsWithdrawnEvent(Guid id, decimal amount) : base(id)
        {
            Amount = amount;
        }
    }
}
