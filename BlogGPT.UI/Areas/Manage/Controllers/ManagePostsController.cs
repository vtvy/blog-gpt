using BlogGPT.Application.Categories.Queries;
using BlogGPT.Application.Posts;
using BlogGPT.Application.Posts.Commands;
using BlogGPT.Application.Posts.Queries;
using BlogGPT.Domain.Constants;
using BlogGPT.UI.Areas.Manage.Models.Category;
using BlogGPT.UI.Areas.Manage.Models.Post;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogGPT.UI.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = Roles.Administrator + "," + Roles.Editor)]
    public class ManagePostsController(IMediator mediator, IMapper mapper, IPostService postService) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly IPostService _postService = postService;

        [TempData]
        public string Status { set; get; } = string.Empty;

        // GET: Posts
        public async Task<IActionResult> IndexAsync(
            string search = "",
            int pageNumber = 1,
            string order = "date",
            string direction = "desc",
            string pageSize = "10",
            string[]? categories = null
        )
        {
            _ = int.TryParse(pageSize, out int pageSizeInput);

            var pagingList = await _mediator.Send(new GetAllManagePostQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSizeInput,
                Categories = categories ?? new string[] { },
                Search = search,
                Order = order,
                Direction = direction,
            });

            var selectCategories = await _mediator.Send(new GetSelectCategoryQuery());

            var categoriesList = _mapper.Map<IEnumerable<TreeModel<SelectCategoryModel>>>(selectCategories);
            var selectList = new List<SelectCategoryModel>();

            CreatePrefixForSelect(categoriesList, selectList, 0);

            ViewBag.pagingModel = new PaginatedModel
            {
                PageNumber = pagingList.PageNumber,
                TotalCount = pagingList.TotalCount,
                TotalPages = pagingList.TotalPages,
                PageSize = pageSize,
                Categories = categories,
                Search = search,
                Order = order,
                Direction = direction,
                CategoryList = selectList
            };

            var postList = _mapper.Map<IReadOnlyCollection<IndexPostModel>>(pagingList.Items);

            return View(postList);
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
                return RedirectToAction("detail", new { id = postId });
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

                var postId = await _mediator.Send(command);

                Status = "Update a post successfully!";
                return RedirectToAction("detail", new { id = postId });
            }

            Status = "Update fail!";
            return RedirectToAction("Edit", post);
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

        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportAsync(string type = "import")
        {
            bool result;
            switch (type)
            {
                case "import":
                    result = await _postService.ImportPostAsync();
                    break;
                case "embedding":
                    result = await _postService.EmbeddingPostAsync();
                    break;
                case "delete":
                    result = await _postService.DeleteEmbeddingPostAsync();
                    break;
                default:
                    Status = $"""The action "{type}" is not found!""";
                    return RedirectToAction("Import");
            }

            Status = result ? $"""The action "{type}" is successfully!""" : $"""The action "{type}" is fail!""";
            return RedirectToAction("Import");
        }

        //[HttpPost]
        //public async Task<IActionResult> UploadAsync(IFormFile file, CancellationToken cancellationToken)
        //{
        //    var imgPath = string.Empty;
        //    if (file.Length > 0)
        //    {
        //        string storedPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, $"wwwroot/files/{_user.UserName}"));
        //        if (!Directory.Exists(storedPath))
        //        {
        //            Directory.CreateDirectory(storedPath);
        //        }
        //        var imgName = DateTime.Now.ToString("yyyyMMddTHHmmss") + file.FileName;
        //        string fullPath = Path.GetFullPath(Path.Combine(storedPath, imgName));
        //        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(fileStream, cancellationToken);
        //        }
        //        imgPath = $@"/files/{_user.UserName}/{imgName}";
        //    }
        //    else
        //    {
        //        imgPath = $"/default.png";
        //    }
        //    return Ok(new { imgPath });
        //}

        private void CreatePrefixForSelect(IEnumerable<TreeModel<SelectCategoryModel>> rawCategories, List<SelectCategoryModel> categoriesSelect, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("--- ", level));
            foreach (var category in rawCategories)
            {
                categoriesSelect.Add(new SelectCategoryModel()
                {
                    Id = category.Item.Id,
                    Name = prefix + category.Item.Name,
                    Slug = category.Item.Slug
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