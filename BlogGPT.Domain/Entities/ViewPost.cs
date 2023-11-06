namespace BlogGPT.Domain.Entities
{
    public class ViewPost
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string? ViewerId { get; set; }

        public Post Post { get; set; } = null!;

        public DateTime ViewedAt { get; set; }
    }
}