using Common.Infrastructure.Core.BaseModels;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public record DepositFundsCommand(Guid Id, decimal Amount) : Message(Id), ICommand;
}
