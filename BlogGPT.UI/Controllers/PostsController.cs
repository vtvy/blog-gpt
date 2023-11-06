using BlogGPT.Application.Categories.Queries;
using BlogGPT.Application.Common.Interfaces.Identity;
using BlogGPT.Application.Images;
using BlogGPT.Application.Posts.Commands;
using BlogGPT.Application.Posts.Queries;
using BlogGPT.Domain.Constants;
using BlogGPT.Infrastructure.Data;
using BlogGPT.UI.ViewModels.Category;
using BlogGPT.UI.ViewModels.Post;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogGPT.UI.Controllers
{
    [Authorize(Roles = Roles.Administrator + "," + Roles.Editor)]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUser _user;
        private readonly IImageService _imageService;

        public PostsController(ApplicationDbContext context, IMediator mediator, IMapper mapper, IUser user, IImageService imageService)
        {
            _context = context;
            _mediator = mediator;
            _mapper = mapper;
            _user = user;
            _imageService = imageService;
        }

        [TempData]
        public string Status { set; get; } = string.Empty;

        // GET: Posts
        [Route("/posts")]
        public async Task<IActionResult> IndexAsync(int pageNumber = 1, int pageSize = 10)
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
        public async Task<IActionResult> DetailAsync(int id)
        {
            var post = await _mediator.Send(new GetDetailPostQuery { Id = id });
            if (post == null)
            {
                return NotFound();
            }

            var postModel = _mapper.Map<DetailPostModel>(post);

            return View(postModel);
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
        public async Task<IActionResult> UploadAsync(IFormFile file, CancellationToken cancellationToken)
        {
            var imgPath = string.Empty;
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
                await _imageService.UploadImageAsync(file.FileName, imgPath, cancellationToken);
            }
            else
            {
                imgPath = $"/default.png";
            }
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