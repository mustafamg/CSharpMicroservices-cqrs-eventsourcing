using Core.Commands;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public class CloseAccountCommand : BaseCommand, ICommand
    {
        public CloseAccountCommand(Guid id) : base(id) { }
    }
}
