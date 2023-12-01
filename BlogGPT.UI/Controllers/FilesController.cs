using BlogGPT.Application.Common.Interfaces.Identity;
using BlogGPT.Application.Common.Security;
using BlogGPT.UI.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BlogGPT.UI.Controllers
{
    [Authorize(Roles = Roles.Administrator + "," + Roles.Editor)]
    public class FilesController(IUser user) : Controller
    {
        private readonly IUser _user = user;

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file, CancellationToken cancellationToken)
        {
            string? imgPath;
            if (file.Length > 0)
            {
                string storedPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, $"wwwroot/files/{_user.UserName}"));
                if (!Directory.Exists(storedPath))
                {
                    Directory.CreateDirectory(storedPath);
                }
                var imgName = DateTime.Now.ToString("yyyyMMddTHHmmss") + file.FileName;
                string fullPath = Path.GetFullPath(Path.Combine(storedPath, imgName));
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream, cancellationToken);
                }
                imgPath = $@"/files/{_user.UserName}/{imgName}";
            }
            else
            {
                imgPath = $"/default.png";
            }
            return Ok(new { imgPath });
        }
    }
}
