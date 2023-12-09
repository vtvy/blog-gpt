namespace BlogGPT.Domain.Entities
{
    public class Message : BaseEntity
    {
        public required string Question { get; set; }
        public required string Embedding { get; set; }

        public required string Answer { get; set; }
    }
}