using BlogGPT.Application.Categories.Queries;
using BlogGPT.Application.Posts.Queries;
using BlogGPT.UI.Areas.Manage.Models.Category;
using BlogGPT.UI.Areas.Manage.Models.Post;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogGPT.UI.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IMediator mediator, IMapper mapper) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        public async Task<IActionResult> IndexAsync()
        {
            var latestPosts = await _mediator.Send(new GetAllPostQuery
            {
                Order = "date"
            });

            ViewBag.latestPosts = _mapper.Map<IReadOnlyCollection<IndexPostModel>>(latestPosts.Items);

            var popularPosts = await _mediator.Send(new GetAllPostQuery
            {
                Order = "view"
            });

            ViewBag.popularPosts = _mapper.Map<IReadOnlyCollection<IndexPostModel>>(popularPosts.Items);

            var categories = await _mediator.Send(new GetCategoryListQuery());

            ViewBag.categories = _mapper.Map<IEnumerable<IndexCategoryModel>>(categories);

            return View();
        }

        public IActionResult PrivacyAsync()
        {


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}