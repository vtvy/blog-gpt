using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Areas.Identity.Models.User
{
    public class SetUserPasswordModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, ErrorMessage = "{0} phải dài {2} đến {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Lặp lại mật khẩu không chính xác.")]
        public string ConfirmPassword { get; set; }


    }
}