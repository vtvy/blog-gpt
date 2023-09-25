using BlogGPT.Domain.Common;

namespace BlogGPT.Domain.Entities
{
    public class Image : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Url = default!;
        public string AuthorId { get; set; } = default!;
        public User Author { get; set; } = default!;
    }
}
