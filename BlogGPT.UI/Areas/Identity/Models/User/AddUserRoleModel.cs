using BlogGPT.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace BlogGPT.UI.Areas.Identity.Models.User
{
    public class AddUserRoleModel
    {
        public ApplicationUser User { get; set; }

        [DisplayName("Các role gán cho user")]
        public string[] RoleNames { get; set; }

        public List<IdentityRoleClaim<string>> ClaimsInRole { get; set; }
        public List<IdentityUserClaim<string>> ClaimsInUserClaim { get; set; }

    }
}