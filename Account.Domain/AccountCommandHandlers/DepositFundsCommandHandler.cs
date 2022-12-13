using Core;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public class DepositFundsCommandHandler : ICommandHandler<DepositFundsCommand>
    {
        readonly EventSourcingHandler<AccountAggregate> _eventSourcingHandler;
        public DepositFundsCommandHandler(EventSourcingHandler<AccountAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }

        public async ValueTask<Unit> Handle(DepositFundsCommand command, CancellationToken cancellationToken)
        {
            var aggregate = await _eventSourcingHandler.GetById(command.Id);
            aggregate.DepositFunds(command.Amount);
            await _eventSourcingHandler.Save(aggregate);
            return new Unit();
        }
    }
}
