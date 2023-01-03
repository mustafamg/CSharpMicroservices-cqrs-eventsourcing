using Core.Commands;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public record CloseAccountCommand(Guid Id) : BaseCommand(Id), ICommand;
}
