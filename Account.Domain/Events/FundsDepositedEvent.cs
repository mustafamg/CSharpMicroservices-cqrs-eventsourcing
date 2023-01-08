using Common.Infrastructure.Core.BaseModels;
using Mediator;

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
}
