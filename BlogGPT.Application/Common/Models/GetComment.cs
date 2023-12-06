namespace BlogGPT.Application.Common.Models
{
    public class GetComment
    {
        public string Author { get; set; } = "Anonymous";

        public int Id { get; set; }

        public required string Content { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
}
