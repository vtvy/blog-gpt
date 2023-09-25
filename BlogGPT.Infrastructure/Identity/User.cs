using BlogGPT.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogGPT.Infrastructure.Identity
{
    public class User : IdentityUser
    {
        public string? Avatar { get; set; } = default!;
        public string? Role { get; set; } = default!;
        public ICollection<Article>? Articles { get; set; }
        public ICollection<Image>? Images { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<Category>? Categories { get; set; }
    }
}
