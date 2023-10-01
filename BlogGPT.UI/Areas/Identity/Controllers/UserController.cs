// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using BlogGPT.Domain.Constants;
using BlogGPT.Infrastructure.Data;
using BlogGPT.Infrastructure.Identity;
using BlogGPT.UI.Areas.Identity.Models.User;
using BlogGPT.UI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogGPT.UI.Areas.Identity.Controllers
{

    [Authorize(Roles = Roles.Administrator)]
    [Area("Identity")]
    [Route("/ManageUser/[action]")]
    public class UserController : Controller
    {

        private readonly ILogger<RoleController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ILogger<RoleController> logger, RoleManager<IdentityRole> roleManager, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }



        [TempData]
        public string StatusMessage { get; set; } = string.Empty;

        //
        // GET: /ManageUser/Index
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int pageNumber)
        {
            var model = new UserListModel
            {
                PageNumber = pageNumber
            };

            var qr = _userManager.Users.OrderBy(u => u.UserName);

            model.TotalUsers = await qr.CountAsync();
            model.TotalPages = (int)Math.Ceiling((double)model.TotalUsers / model.pageSize);

            if (model.PageNumber < 1)
                model.PageNumber = 1;
            if (model.PageNumber > model.TotalPages)
                model.PageNumber = model.TotalPages;

            var qr1 = qr.Skip((model.PageNumber - 1) * model.pageSize)
                        .Take(model.pageSize)
                        .Select(u => new UserAndRole()
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                        });

            model.Users = await qr1.ToListAsync();

            foreach (var user in model.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleNames = string.Join(",", roles);
            }

            return View(model);
        }

        // GET: /ManageUser/AddRole/id
        [HttpGet("{id}")]
        public async Task<IActionResult> AddRoleAsync(string id)
        {
            // public SelectList allRoles { get; set; }
            var model = new AddUserRoleModel();
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }

            model.User = await _userManager.FindByIdAsync(id);

            if (model.User == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }

            model.RoleNames = (await _userManager.GetRolesAsync(model.User)).ToArray();

            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.allRoles = new SelectList(roleNames);

            await GetClaims(model);

            return View(model);
        }

        // GET: /ManageUser/AddRole/id
        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoleAsync(string id, [Bind("RoleNames")] AddUserRoleModel model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }

            model.User = await _userManager.FindByIdAsync(id);

            if (model.User == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }
            await GetClaims(model);

            var OldRoleNames = (await _userManager.GetRolesAsync(model.User)).ToArray();

            var deleteRoles = OldRoleNames.Where(r => !model.RoleNames.Contains(r));
            var addRoles = model.RoleNames.Where(r => !OldRoleNames.Contains(r));

            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            ViewBag.allRoles = new SelectList(roleNames);

            var resultDelete = await _userManager.RemoveFromRolesAsync(model.User, deleteRoles);
            if (!resultDelete.Succeeded)
            {
                ModelState.AddModelError(resultDelete);
                return View(model);
            }

            var resultAdd = await _userManager.AddToRolesAsync(model.User, addRoles);
            if (!resultAdd.Succeeded)
            {
                ModelState.AddModelError(resultAdd);
                return View(model);
            }


            StatusMessage = $"Vừa cập nhật role cho user: {model.User.UserName}";

            return RedirectToAction("Index");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SetPasswordAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }

            var user = await _userManager.FindByIdAsync(id);
            ViewBag.user = ViewBag;

            if (user == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }

            return View();
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPasswordAsync(string id, SetUserPasswordModel model)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound($"Không có user");
            }

            var user = await _userManager.FindByIdAsync(id);
            ViewBag.user = ViewBag;

            if (user == null)
            {
                return NotFound($"Không thấy user, id = {id}.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _userManager.RemovePasswordAsync(user);

            var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            StatusMessage = $"Vừa cập nhật mật khẩu cho user: {user.UserName}";

            return RedirectToAction("Index");
        }


        [HttpGet("{userid}")]
        public async Task<ActionResult> AddClaimAsync(string userid)
        {

            var user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound("Không tìm thấy user");
            ViewBag.user = user;
            return View();
        }

        [HttpPost("{userid}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddClaimAsync(string userid, AddUserClaimModel model)
        {

            var user = await _userManager.FindByIdAsync(userid);
            if (user == null) return NotFound("Không tìm thấy user");
            ViewBag.user = user;
            if (!ModelState.IsValid) return View(model);
            var claims = _context.UserClaims.Where(c => c.UserId == user.Id);

            if (claims.Any(c => c.ClaimType == model.ClaimType && c.ClaimValue == model.ClaimValue))
            {
                ModelState.AddModelError(string.Empty, "Đặc tính này đã có");
                return View(model);
            }

            await _userManager.AddClaimAsync(user, new Claim(model.ClaimType, model.ClaimValue));
            StatusMessage = "Đã thêm đặc tính cho user";

            return RedirectToAction("AddRole", new { id = user.Id });
        }

        [HttpGet("{claimid}")]
        public async Task<IActionResult> EditClaim(int claimid)
        {
            var userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(userclaim.UserId);

            if (user == null) return NotFound("Không tìm thấy user");

            var model = new AddUserClaimModel()
            {
                ClaimType = userclaim.ClaimType,
                ClaimValue = userclaim.ClaimValue

            };
            ViewBag.user = user;
            ViewBag.userclaim = userclaim;
            return View("AddClaim", model);
        }
        [HttpPost("{claimid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClaim(int claimid, AddUserClaimModel model)
        {
            var userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(userclaim.UserId);
            if (user == null) return NotFound("Không tìm thấy user");

            if (!ModelState.IsValid) return View("AddClaim", model);

            if (_context.UserClaims.Any(c => c.UserId == user.Id
                && c.ClaimType == model.ClaimType
                && c.ClaimValue == model.ClaimValue
                && c.Id != userclaim.Id))
            {
                ModelState.AddModelError("Claim này đã có");
                return View("AddClaim", model);
            }


            userclaim.ClaimType = model.ClaimType;
            userclaim.ClaimValue = model.ClaimValue;

            await _context.SaveChangesAsync();
            StatusMessage = "Bạn vừa cập nhật claim";


            ViewBag.user = user;
            ViewBag.userclaim = userclaim;
            return RedirectToAction("AddRole", new { id = user.Id });
        }
        [HttpPost("{claimid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClaimAsync(int claimid)
        {
            var userclaim = _context.UserClaims.Where(c => c.Id == claimid).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(userclaim.UserId);

            if (user == null) return NotFound("Không tìm thấy user");

            await _userManager.RemoveClaimAsync(user, new Claim(userclaim.ClaimType, userclaim.ClaimValue));

            StatusMessage = "Bạn đã xóa claim";

            return RedirectToAction("AddRole", new { id = user.Id });
        }

        private async Task GetClaims(AddUserRoleModel model)
        {
            var listRoles = from r in _context.Roles
                            join ur in _context.UserRoles on r.Id equals ur.RoleId
                            where ur.UserId == model.User.Id
                            select r;

            var _claimsInRole = from c in _context.RoleClaims
                                join r in listRoles on c.RoleId equals r.Id
                                select c;
            model.ClaimsInRole = await _claimsInRole.ToListAsync();


            model.ClaimsInUserClaim = await (from c in _context.UserClaims
                                             where c.UserId == model.User.Id
                                             select c).ToListAsync();

        }
    }
}
