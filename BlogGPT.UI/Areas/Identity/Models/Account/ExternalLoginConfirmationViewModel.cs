// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Areas.Identity.Models.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "Phải đúng định dạng email")]
        public string Email { get; set; }
    }
}
