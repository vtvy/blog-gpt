using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Areas.Identity.Models.Role
{
    public class CreateRoleModel
    {
        [Display(Name = "Tên của role")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải dài {2} đến {1} ký tự")]
        public string Name { get; set; }


    }
}
