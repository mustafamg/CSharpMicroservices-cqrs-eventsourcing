using Common.Infrastructure.Core.BaseModels;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public record WithdrawFundsCommand(Guid Id, decimal Amount) : Message(Id), ICommand;
}
