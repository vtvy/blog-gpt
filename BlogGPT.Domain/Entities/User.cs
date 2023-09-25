using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogGPT.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? Avatar { get; set; } = default!;
        public ICollection<Article>? Articles { get; set; }
        public ICollection<Image>? Images { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<Category>? Categories { get; set; }
    }
}
