// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Areas.Identity.Models.Manage
{
    public class VerifyPhoneNumberViewModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Mã xác nhận")]
        public string Code { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Phone]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
    }
}
