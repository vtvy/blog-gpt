// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Identity;

namespace BlogGPT.UI.Areas.Identity.Models.Role
{
    public class RoleModel : IdentityRole
    {
        public string[] Claims { get; set; }

    }
}
