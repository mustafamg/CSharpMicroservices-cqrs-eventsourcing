using Core;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public record CloseAccountCommand(Guid Id) : Message(Id), ICommand;
}
