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
    public class OpenAccountController : ControllerBase
    {

        private readonly ILogger<OpenAccountController> logger;
        private IMediator dispatcher;

        public OpenAccountController(ILogger<OpenAccountController> logger, IMediator dispatcher)
        {
            this.logger = logger;
            this.dispatcher = dispatcher;
        }

        [HttpPost("OpenAccount")]
        public async Task<bool> OpenAccount(OpenAccountCommand command)
        {
            command.Id = Guid.NewGuid();
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
                var msg = string.Format("Error while processing request to open a new bank account for id - {0}.", command.Id);
                logger.LogCritical(msg, e);
                throw;
            }
        }
    }
}