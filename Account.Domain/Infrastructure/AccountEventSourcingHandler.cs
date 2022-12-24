using Account.Domain;
using Core.Repositories;
using Cqrs.Core;

namespace Core
{
    public class AccountEventSourcingHandler : BaseEventSourcingHandler<AccountAggregate>
    {
        public AccountEventSourcingHandler(IEventStore eventStore) : base(eventStore)
        { }
    }
}
