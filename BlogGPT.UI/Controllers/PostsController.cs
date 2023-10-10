using BlogGPT.Application.Categories.Queries;
using BlogGPT.Application.Posts.Commands;
using BlogGPT.Domain.Constants;
using BlogGPT.Domain.Entities;
using BlogGPT.Infrastructure.Data;
using BlogGPT.UI.ViewModels.Category;
using BlogGPT.UI.ViewModels.Post;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogGPT.UI.Controllers
{
    [Authorize(Roles = Roles.Administrator + "," + Roles.Editor)]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostsController(ApplicationDbContext context, IMediator mediator, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mediator = mediator;
            _mapper = mapper;
            _userManager = userManager;
        }

        [TempData]
        public string Status { set; get; } = string.Empty;

        // GET: Posts
        [Route("/posts")]
        public async Task<IActionResult> Index([FromQuery(Name = "p")] int pageNumber, int pageSize)
        {
            var posts = _context.Posts.OrderByDescending(post => post.LastModifiedAt);

            if (pageSize < 1) pageSize = 10;

            int postNumber = await posts.CountAsync();

            int totalPages = (int)Math.Ceiling((double)postNumber / pageSize);

            if (pageNumber > totalPages) pageNumber = totalPages;
            if (pageNumber < 1) pageNumber = 1;



            var pagingModel = new ViewModels.PaginatedModel<Post>()
            {
                PageNumber = pageNumber,
                TotalPages = totalPages,
                GenerateUrl = (p) => Url.Action("Index", new
                {
                    p,
                    pageSize
                })
            };
            ViewBag.postIndex = (pageNumber - 1) * pageSize;
            ViewBag.pagingModel = pagingModel;
            ViewBag.postNumber = postNumber;
            var pagingPosts = await posts.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Include(p => p.PostCategories)
                    .ThenInclude(post => post.Category)
                    .ToListAsync();

            return View(pagingPosts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public async Task<IActionResult> CreateAsync()
        {
            var categories = await _mediator.Send(new GetSelectCategoryQuery());

            var categoriesList = _mapper.Map<IEnumerable<TreeModel<SelectCategoryModel>>>(categories);
            var selectList = new List<SelectCategoryModel>();

            CreatePrefixForSelect(categoriesList, selectList, 0);

            ViewData["categories"] = new MultiSelectList(selectList, "Id", "Name");
            return View();

        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePostModel post)
        {



            //post.Slug ??= Utility.GenerateSlug(post.Title);

            //if (await _context.Posts.AnyAsync(p => p.Slug == post.Slug))
            //{
            //    ModelState.AddModelError("Slug", "Nhập chuỗi Url khác");
            //    return View(post);
            //}

            if (ModelState.IsValid)
            {
                var command = _mapper.Map<CreatePostCommand>(post);
                var postId = await _mediator.Send(command);

                Status = "Create a blog post successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        [Route("/posts/edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null || _context.Posts == null)
            //{
            //    return NotFound();
            //}

            //var post = await _context.Posts
            //                .Include(post => post.PostCategories)
            //                .FirstOrDefaultAsync(post => post.Id == id);

            //if (post == null)
            //{
            //    return NotFound();
            //}

            //var editPost = new CreatePost()
            //{
            //    Id = post.Id,
            //    Title = post.Title,
            //    Description = post.Description,
            //    Content = post.Content,
            //    Slug = post.Slug,
            //    Published = post.Published,
            //    CategoryIDs = post.PostCategories.Select(post => post.CategoryID).ToArray()
            //};


            //var categories = await _context.Categories.ToListAsync();
            //ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

            //return View(editPost);
            return View();
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Slug,Content,Published,CategoryIDs")] CreatePostModel post)
        {
            //if (id != post.Id)
            //{
            //    return NotFound();
            //}

            //var categories = await _context.Categories.ToListAsync();
            //ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");

            //post.Slug ??= Utility.GenerateSlug(post.Title);

            //if (await _context.Posts.AnyAsync(p => p.Slug == post.Slug && post.Id != id))
            //{
            //    ModelState.AddModelError("Slug", "Nhập chuỗi Url khác");
            //    return View(post);
            //}
            //if (ModelState.IsValid)
            //{

            //    var updatingPost = await _context.Posts
            //            .Include(post => post.PostCategories)
            //            .FirstOrDefaultAsync(post => post.Id == id);

            //    if (updatingPost == null)
            //    {
            //        return NotFound();
            //    }

            //    updatingPost.Title = post.Title;
            //    updatingPost.Description = post.Description;
            //    updatingPost.Content = post.Content;
            //    updatingPost.Slug = post.Slug;
            //    updatingPost.LastModifiedAt = DateTime.Now;


            //    post.CategoryIDs ??= Array.Empty<int>();

            //    var oldCateIds = updatingPost.PostCategories.Select(c => c.CategoryID).ToArray();
            //    var newCateIds = post.CategoryIDs;

            //    var removeCatePosts = from postCate in updatingPost.PostCategories
            //                          where !newCateIds.Contains(postCate.CategoryID)
            //                          select postCate;
            //    _context.PostCategories.RemoveRange(removeCatePosts);

            //    var addCateIds = from CateId in newCateIds
            //                     where !oldCateIds.Contains(CateId)
            //                     select CateId;

            //    foreach (var CateId in addCateIds)
            //    {
            //        _context.PostCategories.Add(new PostCategory()
            //        {
            //            PostID = id,
            //            CategoryID = CateId
            //        });
            //    }


            //    _context.Update(updatingPost);
            //    await _context.SaveChangesAsync();

            //    Status = "Update a post successfully!";
            //    return RedirectToAction(nameof(Index));
            //}
            //Status = "Update fail!";
            //return View(post);
            return View();
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            Status = "Success delete the post: " + post.Title;

            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            string imgPath;
            string storedPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "wwwroot/files"));
            if (file.Length > 0)
            {
                if (!Directory.Exists(storedPath))
                {
                    Directory.CreateDirectory(storedPath);
                }
                imgPath = DateTime.Now.ToString("yyyyMMddTHHmmss") + file.FileName;
                string fullPath = Path.Combine(storedPath, imgPath);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                imgPath = $@"{storedPath}/{imgPath}";

                return Ok(new { imgPath });
            }
            imgPath = $"{storedPath}/thumnail.png";
            return Ok(new { imgPath });
        }

        private void CreatePrefixForSelect(IEnumerable<TreeModel<SelectCategoryModel>> rawCategories, List<SelectCategoryModel> categoriesSelect, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("--- ", level));
            foreach (var category in rawCategories)
            {
                categoriesSelect.Add(new SelectCategoryModel()
                {
                    Id = category.Item.Id,
                    Name = prefix + category.Item.Name,
                });

                if (category.Children != null)
                {
                    var childLevel = level + 1;
                    CreatePrefixForSelect(category.Children, categoriesSelect, childLevel);
                }
            }
        }
    }
}