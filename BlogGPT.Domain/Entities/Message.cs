namespace BlogGPT.Domain.Entities
{
    public class Message : BaseEntity
    {
        public required string Question { get; set; }

        public string? Answer { get; set; }

        public int ConversationId { get; set; }

        public int? ModelId { get; set; }

        public Conversation? Conversation { get; set; }

        public Model? Model { get; set; }
    }
}