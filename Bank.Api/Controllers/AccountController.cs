using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection.Emit;
using System;
using Core.Commands;
using Account.Domain;
using Account.Domain.AccountCommands;
using System.Net;
using Core.Exceptions;
using Mediator;

namespace Bank.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly ILogger<AccountController> logger;
        private IMediator dispatcher;

        public AccountController(ILogger<AccountController> logger, IMediator dispatcher)
        {
            this.logger = logger;
            this.dispatcher = dispatcher;
        }

        [HttpPost("open")]
        public async Task<bool> Open(OpenAccountCommand command)
        {
            var openAccountCommand = new OpenAccountCommand(
                Guid.NewGuid(), 
                command.AccountHolder,
                command.AccountType,
                command.OpeningBalance);
            return await SendCommand(openAccountCommand);
        }

        [HttpPost("close")]
        public async Task<bool> Close(CloseAccountCommand command)
        {
            return await SendCommand(command);
        }

        [HttpPost("withdraw")]
        public async Task<bool> WithdrawFunds(WithdrawFundsCommand command)
        {
            return await SendCommand(command);
        }
        
        [HttpPost("deposit/{id}")]
        public async Task<bool> DepositFunds(DepositFundsCommand command)
        {
            return await SendCommand(command);
        }

        private async Task<bool> SendCommand<T>(T command) where T: ICommand 
        {
            var cmd = command as BaseCommand;
            try
            {
                await dispatcher.Send(command);
                return true;
            }
            catch (IllegalStateException e)
            {
                var msg = $"Client made a bad request - {e.Message}";
                logger.LogWarning(msg);
                throw new BadHttpRequestException(msg);
            }
            catch (Exception e)
            {
                var msg = string.Format("Error while processing request for {1} with Id id - {0}.", 
                    cmd?.Id, command.GetType().Name);
                logger.LogCritical(msg, e);
                throw;
            }
        }
    }
}