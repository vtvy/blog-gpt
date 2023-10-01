namespace BlogGPT.Domain.Entities
{
    public class Image : BaseAuditableEntity
    {
        public required string Name { get; set; }

        public required string Url { get; set; }
    }
}
