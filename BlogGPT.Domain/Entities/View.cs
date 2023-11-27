namespace BlogGPT.Domain.Entities
{
    public class View
    {
        public int Id { get; set; }
        public int PostId { get; set; }

        public Post Post { get; set; } = null!;

        public int Count { get; set; }
    }
}