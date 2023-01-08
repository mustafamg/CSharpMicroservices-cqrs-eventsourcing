using Account.Domain.Events;
using Infrastructure;
using System.Text.Json;
using Common.Infrastructure.Core.BaseModels;

namespace Account.Domain.Infrastructure
{
    public sealed class AccountEventStoreRepository : BaseEventStoreRepository
    {
        protected override byte[] SerializeBasedOnEventType(BaseEvent @event)
        {
            return @event.GetType().Name switch
            {
                nameof(AccountOpenedEvent) => JsonSerializer.SerializeToUtf8Bytes(@event as AccountOpenedEvent),
                nameof(AccountClosedEvent) => JsonSerializer.SerializeToUtf8Bytes(@event as AccountClosedEvent),
                nameof(FundsDepositedEvent) => JsonSerializer.SerializeToUtf8Bytes(@event as FundsDepositedEvent),
                nameof(FundsWithdrawnEvent) => JsonSerializer.SerializeToUtf8Bytes(@event as FundsWithdrawnEvent),
                _ => throw new ApplicationException("ParseEvent couldn't find the event to parse to"),
            };
        }
    }
}
