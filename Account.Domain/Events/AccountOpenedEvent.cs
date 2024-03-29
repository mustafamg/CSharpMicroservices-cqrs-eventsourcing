﻿using Account.Domain.Dto;
using Common.Infrastructure.Core.BaseModels;
using Mediator;

namespace Account.Domain.Events
{
    internal class AccountOpenedEvent:BaseEvent
    {
        public string AccountHolder { get; private set; }
        public AccountType AccountType { get; private set; }
        public decimal OpeningBalance { get; private set; }
        public DateTime CreatedDate { get; set; }
        public AccountOpenedEvent(
            Guid id,
            string accountHolder,
            AccountType accountType,
            decimal openingBalance,
            DateTime createdDate) : base(id)
        {
            OpeningBalance = openingBalance;
            AccountType = accountType;
            AccountHolder = accountHolder;
            CreatedDate= createdDate;
        }
    }
}
