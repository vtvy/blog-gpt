namespace BlogGPT.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public required string Content { get; set; }

        public required int PostId { get; set; }

        public required Post Post { get; set; }
    }
}
