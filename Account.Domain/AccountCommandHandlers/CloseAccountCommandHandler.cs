using Core;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public class CloseAccountCommandHandler : ICommandHandler<CloseAccountCommand>
    {
        readonly AccountEventSourcingHandler _eventSourcingHandler;
        public CloseAccountCommandHandler(AccountEventSourcingHandler eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public async ValueTask<Unit> Handle(CloseAccountCommand command, CancellationToken cancellationToken)
        {
            var aggregate = await _eventSourcingHandler.GetById(command.Id);
            aggregate.CloseAccount();
            await _eventSourcingHandler.Save(aggregate);
            return new Unit();
        }
    }
}
