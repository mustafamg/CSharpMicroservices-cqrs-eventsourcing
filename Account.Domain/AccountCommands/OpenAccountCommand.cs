using Account.Domain.Dto;
using Core.Commands;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public class OpenAccountCommand : BaseCommand, ICommand
    {
        public string AccountHolder { get; private set; }
        public AccountType AccountType { get; private set; }
        public decimal OpeningBalance { get; private set; }
        public OpenAccountCommand(
            Guid id,
            string accountHolder,
            AccountType accountType,
            decimal openingBalance) : base(id)
        {
            OpeningBalance = openingBalance;
            AccountType = accountType;
            AccountHolder = accountHolder;
        }
    }
}
