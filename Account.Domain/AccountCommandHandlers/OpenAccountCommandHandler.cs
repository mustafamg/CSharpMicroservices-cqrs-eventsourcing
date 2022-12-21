using Core;
using Core.Commands;
using Core.Exceptions;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public class OpenAccountCommandHandler : ICommandHandler<OpenAccountCommand>
    {
        readonly AccountEventSourcingHandler _eventSourcingHandler;
        public OpenAccountCommandHandler(AccountEventSourcingHandler eventSourcingHandler)
        {
            this._eventSourcingHandler = eventSourcingHandler;
        }

        public async ValueTask<Unit> Handle(OpenAccountCommand command, CancellationToken cancellationToken)
        {
            if (command is OpenAccountCommand cmd)
            {
                var aggregate = new AccountAggregate(cmd);
                await _eventSourcingHandler.Save(aggregate);
            }
            return new Unit();
        }
    }
}
