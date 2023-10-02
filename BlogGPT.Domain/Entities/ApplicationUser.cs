﻿using Microsoft.AspNetCore.Identity;

namespace BlogGPT.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Avatar { get; set; } = default!;

        public ICollection<Post>? Posts { get; set; }

        public ICollection<Image>? Images { get; set; }

        public ICollection<Feedback>? Feedbacks { get; set; }

        public ICollection<Category>? Categories { get; set; }
    }
}
