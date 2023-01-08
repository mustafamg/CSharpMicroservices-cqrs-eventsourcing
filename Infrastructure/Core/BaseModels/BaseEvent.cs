namespace Common.Infrastructure.Core.BaseModels
{
    public class BaseEvent
    {
        public BaseEvent(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; protected set; }
        public int Version { get; set; }
    }
}
