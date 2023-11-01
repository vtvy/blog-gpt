using BlogGPT.Application.Categories.Queries;
using BlogGPT.Application.Posts.Commands;
using BlogGPT.Application.Posts.Queries;
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
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var pagingList = await _mediator.Send(new GetAllPostQuery { PageNumber = pageNumber, PageSize = pageSize });

            var pagingModel = new PaginatedModel<IndexPostModel>();
            pagingModel.PageNumber = pagingList.PageNumber;
            pagingModel.TotalCount = pagingList.TotalCount;
            pagingModel.TotalPages = pagingList.TotalPages;
            pagingModel.PageSize = pageSize;

            ViewBag.pagingModel = pagingModel;

            pagingModel.Items = _mapper.Map<IReadOnlyCollection<IndexPostModel>>(pagingList.Items);

            ViewBag.postIndex = (pagingModel.PageNumber - 1) * pageSize;
            ViewBag.postNumber = pagingModel.TotalCount;

            return View(pagingModel);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreatePostModel post)
        {
            if (ModelState.IsValid)
            {
                var command = _mapper.Map<CreatePostCommand>(post);
                var postId = await _mediator.Send(command);

                Status = "Create a blog post successfully!";
                //return RedirectToAction(nameof(Index));
                return View(post);
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> EditAsync(int id)
        {
            var editPost = await _mediator.Send(new GetPostQuery { Id = id });

            if (editPost == null)
            {
                return NotFound();
            }

            var categories = await _mediator.Send(new GetSelectCategoryQuery());

            var categoriesList = _mapper.Map<IEnumerable<TreeModel<SelectCategoryModel>>>(categories);
            var selectList = new List<SelectCategoryModel>();

            CreatePrefixForSelect(categoriesList, selectList, 0);

            ViewData["categories"] = new MultiSelectList(selectList, "Id", "Name");

            var editPostModel = _mapper.Map<EditPostModel>(editPost);
            return View(editPostModel);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, EditPostModel post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var command = _mapper.Map<UpdatePostCommand>(post);

                await _mediator.Send(command);

                Status = "Update a post successfully!";
                //return RedirectToAction(nameof(Index));
                return View(post);
            }

            Status = "Update fail!";
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deletePost = await _mediator.Send(new GetPostQuery { Id = id });

            if (deletePost == null)
            {
                return NotFound();
            }

            var post = _mapper.Map<DeletePostModel>(deletePost);

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(int id)
        {
            await _mediator.Send(new DeletePostCommand { Id = id });
            Status = "Success delete the post";

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