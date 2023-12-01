﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Areas.Identity.Models.Account
{
    public class VerifyAuthenticatorCodeViewModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Nhập mã đã lưu")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Nhớ thông tin trình duyệt này?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Nhớ thông tin đăng nhập?")]
        public bool RememberMe { get; set; }
    }
}
