using BlogGPT.Infrastructure.Identity;

namespace BlogGPT.UI.Areas.Identity.Models.User
{
    public class UserListModel
    {
        public int TotalUsers { get; set; }

        public int TotalPages { get; set; }

        public int pageSize { get; set; } = 10;

        public int PageNumber { get; set; }

        public List<UserAndRole> Users { get; set; } = new();

    }

    public class UserAndRole : ApplicationUser
    {
        public string RoleNames { get; set; } = string.Empty;
    }


}