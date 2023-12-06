namespace BlogGPT.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public required string Content { get; set; }

        public required int PostId { get; set; }

        public Post Post { get; set; } = null!;
    }
}
