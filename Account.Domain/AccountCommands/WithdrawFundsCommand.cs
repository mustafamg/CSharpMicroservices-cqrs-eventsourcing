using Core.Commands;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public record WithdrawFundsCommand(Guid Id, decimal Amount) : BaseCommand(Id), ICommand;
}
