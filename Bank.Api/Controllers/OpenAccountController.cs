using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection.Emit;
using System;
using Core.Commands;
using Account.Domain;
using Account.Domain.AccountCommands;
using System.Net;
using Core.Exceptions;

namespace Bank.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpenAccountController : ControllerBase
    {

        private readonly ILogger<OpenAccountController> _logger;

        public OpenAccountController(ILogger<OpenAccountController> logger, CommandDispatcher commandDispatcher)
        {
            _logger = logger;
            this._commandDispatcher = commandDispatcher;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<int> Get()
        //{
        //    return Enumerable.Range(1, 5).ToArray();
        //}
        private CommandDispatcher _commandDispatcher;

        [HttpPost("OpenAccount")]
        public ActionResult<bool> OpenAccount(OpenAccountCommand command)
        {
            command.Id = Guid.NewGuid();
            try
            {
                _commandDispatcher.Send(command);
                return true;
            }
            catch (IllegalStateException e)
            {
                var msg = $"Client made a bad request - {e.Message}";
                _logger.LogWarning(msg);
                throw new BadHttpRequestException(msg);
            }
            catch (Exception e)
            {
                var msg = string.Format("Error while processing request to open a new bank account for id - {0}.", command.Id);
                _logger.LogCritical(msg, e);
                throw new ApplicationException(msg);
            }
        }
    }
}