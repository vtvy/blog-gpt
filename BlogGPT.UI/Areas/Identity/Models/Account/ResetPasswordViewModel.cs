// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Areas.Identity.Models.Account
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "Phải đúng định dạng email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập mật khẩu mới")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Re-Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }

    }
}
