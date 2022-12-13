using Core.Commands;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public class DepositFundsCommand : BaseCommand, ICommand
    {
        public DepositFundsCommand(Guid id, decimal amount) : base(id) { Amount = amount; }
        public decimal Amount { get; set; }
    }
}
