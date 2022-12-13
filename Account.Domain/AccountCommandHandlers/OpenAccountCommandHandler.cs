using Account.Domain.Dto;
using Core;
using Core.Commands;
using Core.Infrastructure;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public class OpenAccountCommandHandler : ICommandHandler<OpenAccountCommand>
    {
        readonly EventSourcingHandler<AccountAggregate> _eventSourcingHandler;
        public OpenAccountCommandHandler(EventSourcingHandler<AccountAggregate> eventSourcingHandler)
        {
            this._eventSourcingHandler = eventSourcingHandler;
        }

        public void Handle(BaseCommand command)
        {
            if (command is OpenAccountCommand cmd)
            {
                var aggregate = new AccountAggregate(cmd);
                _eventSourcingHandler.Save(aggregate);
            }
        }

        public ValueTask<Unit> Handle(OpenAccountCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
