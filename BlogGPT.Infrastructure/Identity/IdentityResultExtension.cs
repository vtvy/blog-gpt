using BlogGPT.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogGPT.Infrastructure.Identity
{
    public static class IdentityResultExtension
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded ? Result.Success() : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}
