namespace BlogGPT.Domain.Entities
{
    public class Comment : BaseEntity, ITree<Comment>
    {
        public required string Content { get; set; }

        public required int PostId { get; set; }

        public required Post Post { get; set; }

        public int? ParentId { get; set; }

        public Comment? Parent { get; set; }

        public ICollection<Comment>? Children { get; set; }
    }
}
