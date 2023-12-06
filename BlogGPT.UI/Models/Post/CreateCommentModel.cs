namespace BlogGPT.UI.Models.Post
{
    public class CreateCommentModel
    {
        public int Id { get; set; }
        public required string AddComment { get; set; }
        public int? AddCommentId { get; set; }
        public required string Slug { get; set; }
    }
}
