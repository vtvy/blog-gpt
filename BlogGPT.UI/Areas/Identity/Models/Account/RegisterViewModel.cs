// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Areas.Identity.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Display(Name = "Email")]
        public required string Email { get; set; }


        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, ErrorMessage = "{0} must long {2} to {1} characters.", MinimumLength = 2)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Re-Password")]
        [Compare("Password", ErrorMessage = "Re-Password does not match")]
        public required string ConfirmPassword { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, ErrorMessage = "{0} must long {2} to {1} characters.", MinimumLength = 3)]
        public required string UserName { get; set; }

    }
}
