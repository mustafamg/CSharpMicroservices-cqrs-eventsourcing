using Core.Commands;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public class WithdrawFundsCommand : BaseCommand, ICommand
    {
        public WithdrawFundsCommand(Guid id, decimal amount) : base(id) { Amount = amount; }

        public decimal Amount { get; set; }
    }
}
