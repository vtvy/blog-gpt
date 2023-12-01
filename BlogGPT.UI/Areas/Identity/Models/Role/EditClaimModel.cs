using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Areas.Identity.Models.Role
{
    public class EditClaimModel
    {
        [Display(Name = "Kiểu (tên) claim")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài {2} đến {1} ký tự")]
        public string ClaimType { get; set; }

        [Display(Name = "Giá trị")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài {2} đến {1} ký tự")]
        public string ClaimValue { get; set; }

        public IdentityRole role { get; set; }


    }
}
