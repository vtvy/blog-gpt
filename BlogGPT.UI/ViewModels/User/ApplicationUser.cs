﻿using BlogGPT.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.ViewModels.User
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(Lengths.Large)]
        public string? Avatar { set; get; }
    }
}