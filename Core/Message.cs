namespace Core
{
    public abstract class Message
    {
        public Message(Guid id) { Id = id; }
        public Guid Id { get; set; }
    }
}
