using Account.Domain.Dto;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public record OpenAccountCommand(
            string AccountHolder,
            AccountType AccountType,
            decimal OpeningBalance): ICommand;
}
