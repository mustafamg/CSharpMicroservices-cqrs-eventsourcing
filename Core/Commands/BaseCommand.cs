namespace Core.Commands
{
    public abstract class BaseCommand : Message
    {
        public BaseCommand(Guid id) : base(id) { }
    }
}
