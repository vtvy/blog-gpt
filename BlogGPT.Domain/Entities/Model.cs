namespace BlogGPT.Domain.Entities
{
    public class Model : BaseEntity
    {
        public required string Name { get; set; }

        public ICollection<Message>? Messages { get; set; }
    }
}