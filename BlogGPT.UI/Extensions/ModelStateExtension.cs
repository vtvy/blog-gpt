using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogGPT.UI.Extensions
{
    public static class ModelStateExtension
    {
        public static void AddModelError(this ModelStateDictionary ModelState, string message)
        {
            ModelState.AddModelError("Message", message);
        }
        public static void AddModelError(this ModelStateDictionary ModelState, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Description);
            }
        }
    }
}
