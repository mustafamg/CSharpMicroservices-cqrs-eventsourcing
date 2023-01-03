using Core.Commands;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public record DepositFundsCommand(Guid Id, decimal Amount) : BaseCommand(Id), ICommand;
}
