using Microsoft.AspNetCore.Identity;

namespace BlogGPT.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Avatar { get; set; } = default!;

        public ICollection<Post>? Posts { get; set; }

        public ICollection<Image>? Images { get; set; }

        public ICollection<Comment>? Comments { get; set; }

        public ICollection<Category>? Categories { get; set; }

        public ICollection<Conversation>? Conversations { get; set; }

        public ICollection<Message>? Messages { get; set; }

    }
}
