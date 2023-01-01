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
            command.Id = Guid.NewGuid();
            return await SendCommand(command);
        }

        [HttpPost("close/{id}")]
        public async Task<bool> Close(Guid id, CloseAccountCommand command)
        {
            command.Id = id;
            return await SendCommand(command);
        }

        [HttpPost("withdraw/{id}")]
        public async Task<bool> WithdrawFunds(Guid id, WithdrawFundsCommand command)
        {
            command.Id = id;
            return await SendCommand(command);
        }
        
        [HttpPost("deposit/{id}")]
        public async Task<bool> DepositFunds(Guid id, DepositFundsCommand command)
        {
            command.Id = id;
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