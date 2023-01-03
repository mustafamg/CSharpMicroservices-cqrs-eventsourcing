using Account.Domain.Dto;
using Core.Commands;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public record OpenAccountCommand(
            Guid Id,
            string AccountHolder,
            AccountType AccountType,
            decimal OpeningBalance) : BaseCommand(Id), ICommand;
}
