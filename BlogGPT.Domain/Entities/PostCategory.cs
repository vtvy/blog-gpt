namespace BlogGPT.Domain.Entities
{
    public class PostCategory
    {
        public int PostId { get; set; }

        public int CategoryId { get; set; }

        public required Post Post { get; set; }

        public required Category Category { get; set; }
    }
}
