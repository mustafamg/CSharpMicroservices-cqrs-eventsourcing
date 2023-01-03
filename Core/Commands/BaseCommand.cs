namespace Core.Commands
{
    public abstract record BaseCommand(Guid Id) : Message(Id);
}
