namespace BlogGPT.Domain.Entities
{
    public class Image : BaseEntity
    {
        public required string Name { get; set; }

        public required string Url { get; set; }
    }
}
