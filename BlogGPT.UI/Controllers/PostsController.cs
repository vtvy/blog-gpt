using BlogGPT.Application.Categories.Queries;
using BlogGPT.Application.Posts.Queries;
using BlogGPT.Infrastructure.Data;
using BlogGPT.UI.Models.Post;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlogGPT.UI.Controllers
{
    public class PostsController : Controller
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PostsController(ILogger<PostsController> logger, ApplicationDbContext context, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        // /posts/
        public async Task<IActionResult> IndexAsync(
                    string search = "",
                    int pageNumber = 1,
                    int pageSize = 10,
                    string categories = "",
                    string order = "date",
                    string direction = "desc")
        {
            var pagingList = await _mediator.Send(new GetAllPostQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Categories = categories,
                Search = search,
                Order = order,
                Direction = direction,
            });

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
            };

            var postList = _mapper.Map<IReadOnlyCollection<IndexPostModel>>(pagingList.Items);

            var categoryTree = await _mediator.Send(new GetAllCategoryQuery());
            ViewBag.categories = _mapper.Map<IEnumerable<TreeModel<CategoryModel>>>(categoryTree);
            return View(postList);
        }

        [Route("/posts/{slug}.html")]
        public async Task<IActionResult> DetailAsync(string slug)
        {
            var post = await _mediator.Send(new GetDetailPostBySlugQuery { Slug = slug });
            if (post == null)
            {
                return NotFound();
            }

            var postModel = _mapper.Map<DetailPostModel>(post);

            var categoryTree = await _mediator.Send(new GetAllCategoryQuery());
            ViewBag.categories = _mapper.Map<IEnumerable<TreeModel<CategoryModel>>>(categoryTree);

            //ViewBag.otherPosts = otherPosts;

            return View(postModel);
        }
    }
}