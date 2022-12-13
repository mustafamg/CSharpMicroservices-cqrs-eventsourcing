using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Infrastructure;

namespace Core.Commands
{
    public interface ICommandDispatcher
    {
        void RegisterHandler<T>(ICommandHandler handler) where T : BaseCommand;
        void Send<T>(T command) where T : BaseCommand;
    }

    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Dictionary<string, ICommandHandler> handlers = new();
        public void RegisterHandler<T>(ICommandHandler handler) where T : BaseCommand
        {
            if (handlers.ContainsKey(typeof(T).Name))
            {
                throw new Exception("Cant register more than one handler for the same command");
            }

            handlers.Add(typeof(T).Name, handler);
        }

        public void Send<T>(T command) where T : BaseCommand
        {
            var handler = handlers[typeof(T).Name];
            if (handler == null)
            {
                throw new ApplicationException("No command handler was registered!");
            }
            handler.Handle(command);
        }
    }
}
