﻿using Account.Domain.AccountCommands;
using Account.Domain.Dto;
using Account.Domain.Events;
using Core.Exceptions;
using Cqrs.Core;
using Microsoft.Extensions.Logging;

namespace Account.Domain
{
    public class AccountAggregate : AggregateRoot
    {
        public string AccountHolder { get; set; }
        public decimal Balance { get; private set; }
        public bool Active { get; private set; }
        public AccountType AccountType { get; set; }

        public AccountAggregate()
        {         
            //todo: do not use, created only to enale new T on GetById method of eventsourcing handler
        }

        /// <summary>
        /// Factory function to create the account aggregate
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="IllegalStateException"></exception>
        public static AccountAggregate OpenAccount(OpenAccountCommand command)
        {
            if(command.OpeningBalance <= 1000)
            {
                throw new IllegalStateException("Openning balance should be greater than 1000");
            }

            var aggregate = new AccountAggregate();

            aggregate.RaiseEvent(new AccountOpenedEvent(
                        id: Guid.NewGuid(),
                        accountHolder: command.AccountHolder,
                        createdDate: DateTime.Now,
                        accountType: command.AccountType,
                        openingBalance: command.OpeningBalance));
            return aggregate;
        }

        private void Apply(AccountOpenedEvent evnt)
        {
            this.Id = evnt.Id;
            this.AccountHolder = evnt.AccountHolder;
            this.Active = true;
            this.Balance = evnt.OpeningBalance;
            this.AccountType = evnt.AccountType;
        }

        public void DepositFunds(decimal amount)
        {
            if (!this.Active)
            {
                throw new IllegalStateException("Funds cannot be deposited into a closed account!");
            }
            if (amount <= 0)
            {
                throw new IllegalStateException("The deposit amount must be greater than 0!");
            }
            RaiseEvent(new FundsDepositedEvent(
                        id: this.Id,
                        amount: amount));
        }

        private void Apply(FundsDepositedEvent evnt)
        {
            this.Balance += evnt.Amount;
        }

        public void WithdrawFunds(decimal amount)
        {
            if (amount > this.Balance)
            {
                throw new IllegalStateException("Withdrawal declined, insufficient funds!");
            }
            if (!this.Active)
            {
                throw new IllegalStateException("Funds cannot be withdrawn from a closed account!");
            }

            RaiseEvent(new FundsWithdrawnEvent(
                        id: this.Id,
                        amount: amount));
        }

        private void Apply(FundsWithdrawnEvent evnt)
        {
            this.Balance -= evnt.Amount;
        }

        public void CloseAccount()
        {
            //TODO: Define what should happen to satisify the command
            if (!this.Active)
            {
                throw new IllegalStateException("The bank account has already been closed!");
            }
            //TODO: what the event resulted from such command
            RaiseEvent(new AccountClosedEvent(
                        id: this.Id));
        }

        /// <summary>
        /// Apply just set event state
        /// </summary>
        /// <param name="evnt"></param>
        private void Apply(AccountClosedEvent evnt)
        {
            this.Active = false;
        }
    }
}
