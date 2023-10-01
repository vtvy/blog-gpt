namespace BlogGPT.Domain.Entities
{
    public class Feedback : BaseAuditableEntity
    {
        public required string Content { get; set; }
    }
}
