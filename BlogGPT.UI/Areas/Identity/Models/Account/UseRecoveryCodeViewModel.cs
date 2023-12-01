// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Areas.Identity.Models.Account
{
    public class UseRecoveryCodeViewModel
    {

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Nhập mã phục hồi đã lưu")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }
    }
}
