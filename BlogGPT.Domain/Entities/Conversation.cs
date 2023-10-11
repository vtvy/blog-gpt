namespace BlogGPT.Domain.Entities
{
    public class Conversation : BaseEntity
    {
        public bool IsOver { get; set; }

        public ICollection<Message>? Messages { get; set; }
    }
}