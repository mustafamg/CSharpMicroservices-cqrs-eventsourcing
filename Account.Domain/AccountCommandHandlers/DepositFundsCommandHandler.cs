using Core;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public class DepositFundsCommandHandler : ICommandHandler<DepositFundsCommand>
    {
        readonly AccountEventSourcingHandler _eventSourcingHandler;
        public DepositFundsCommandHandler(AccountEventSourcingHandler eventSourcingHandler)
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
