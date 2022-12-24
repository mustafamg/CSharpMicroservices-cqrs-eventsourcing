using Core.Events;
using Account.Domain.Events;
using System.Text.Json;
using Infrastructure;

namespace Account.Domain.Infrastructure
{
    public class AccountEventStore : BaseEventStore
    {
        public AccountEventStore(BaseEventStoreRepository eventStoreRepository) :
            base(eventStoreRepository, nameof(AccountAggregate))
        { }

        protected override async Task<BaseEvent?> ParseEvent(EventModel eventModel)
        {
            //var dataStr = Encoding.UTF8.GetString(eventModel.EventData.ToArray());
            return eventModel.EventType switch
            {
                nameof(AccountOpenedEvent) => await JsonSerializer.DeserializeAsync<AccountOpenedEvent>(
                                        new MemoryStream(eventModel.EventData.ToArray())),
                nameof(AccountClosedEvent) => await JsonSerializer.DeserializeAsync<AccountClosedEvent>(
                                    new MemoryStream(eventModel.EventData.ToArray())),
                nameof(FundsDepositedEvent) => await JsonSerializer.DeserializeAsync<FundsDepositedEvent>(
                                    new MemoryStream(eventModel.EventData.ToArray())),
                nameof(FundsWithdrawnEvent) => await JsonSerializer.DeserializeAsync<FundsWithdrawnEvent>(
                                    new MemoryStream(eventModel.EventData.ToArray())),
                _ => throw new ApplicationException("ParseEvent couldn't find the event to parse to"),
            };
        }
    }
}