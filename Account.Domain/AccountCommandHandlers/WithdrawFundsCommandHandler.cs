using Core;
using Core.Exceptions;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public class WithdrawFundsCommandHandler : ICommandHandler<WithdrawFundsCommand>
    {
        readonly EventSourcingHandler<AccountAggregate> _eventSourcingHandler;
        public WithdrawFundsCommandHandler(EventSourcingHandler<AccountAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }
        public async ValueTask<Unit> Handle(WithdrawFundsCommand command, CancellationToken cancellationToken)
        {
            var aggregate = await _eventSourcingHandler.GetById(command.Id);
            if (command.Amount > aggregate.Balance)
            {
                throw new IllegalStateException("Withdrawal declined, insufficient funds!");
            }
            aggregate.WithdrawFunds(command.Amount);
            await _eventSourcingHandler.Save(aggregate);
            return new Unit();
        }
    }
}
