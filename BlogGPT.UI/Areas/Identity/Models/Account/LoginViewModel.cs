// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Areas.Identity.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Username or email")]
        public required string UserNameOrEmail { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
