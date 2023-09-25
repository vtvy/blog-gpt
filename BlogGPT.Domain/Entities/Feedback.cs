using BlogGPT.Domain.Common;

namespace BlogGPT.Domain.Entities
{
    public class Feedback : BaseEntity
    {
        public string Content { get; set; } = default!;
        public string AuthorId { get; set; } = default!;
        public User Author { get; set; } = default!;
    }
}
