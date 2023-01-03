using Core;
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
            var aggregate = AccountAggregate.OpenAccount(command);
            await _eventSourcingHandler.Save(aggregate);
            return new Unit();
        }
    }
}
