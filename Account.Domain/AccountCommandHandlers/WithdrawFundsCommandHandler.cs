﻿using Core;
using Core.Exceptions;
using Mediator;

namespace Account.Domain.AccountCommands
{
    public class WithdrawFundsCommandHandler : ICommandHandler<WithdrawFundsCommand>
    {
        readonly AccountEventSourcingHandler _eventSourcingHandler;
        public WithdrawFundsCommandHandler(AccountEventSourcingHandler eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }
        public async ValueTask<Unit> Handle(WithdrawFundsCommand command, CancellationToken cancellationToken)
        {
            var aggregate = await _eventSourcingHandler.GetById(command.Id);
            aggregate.WithdrawFunds(command.Amount);
            await _eventSourcingHandler.Save(aggregate);
            return new Unit();
        }
    }
}
