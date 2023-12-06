using BlogGPT.Application.Categories.Commands;
using BlogGPT.Application.Categories.Queries;
using BlogGPT.Application.Posts.Queries;
using BlogGPT.UI.Models.Post;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogGPT.UI.Controllers
{
	[Authorize]
	public class PostsController(ILogger<PostsController> logger, IMediator mediator, IMapper mapper) : Controller
	{
		private readonly ILogger<PostsController> _logger = logger;
		private readonly IMediator _mediator = mediator;
		private readonly IMapper _mapper = mapper;

		// /posts/
		[AllowAnonymous]
		public async Task<IActionResult> IndexAsync(
					string search = "",
					int pageNumber = 1,
					string pageSize = "10",
					string[]? categories = null,
					string order = "date",
					string direction = "desc")
		{
			_ = int.TryParse(pageSize, out int pageSizeInput);

			categories ??= [];

			var pagingList = await _mediator.Send(new GetAllPostQuery
			{
				PageNumber = pageNumber,
				PageSize = pageSizeInput,
				Categories = categories ?? [],
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

		[AllowAnonymous]
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

			var similarPosts = await _mediator.Send(new GetSimilarPostQuery { Id = post.Id });

			ViewBag.similarPosts = similarPosts.Any() ? _mapper.Map<IEnumerable<SimilarPost>>(similarPosts) : null;

			return View(postModel);
		}

		[HttpPost]
		public async Task<IActionResult> CommentAsync(CreateCommentModel comment)
		{
			if (comment.AddCommentId != null && comment.AddCommentId != 0)
			{
				var commentId = await _mediator.Send(new EditCommentCommand { Id = (int)comment.AddCommentId, PostId = comment.Id, Comment = comment.AddComment });

				if (commentId == -1)
				{
					return BadRequest();
				}
			}
			else
			{
				var commentId = await _mediator.Send(new CreateCommentCommand { PostId = comment.Id, Comment = comment.AddComment });

				if (commentId == -1)
				{
					return BadRequest();
				}
			}

			return Redirect($"/posts/{comment.Slug}.html");
		}

		[HttpDelete]
		public async Task<IActionResult> CommentAsync(int id)
		{
			var commentId = await _mediator.Send(new DeleteCommentCommand { Id = id });

			if (commentId == -1)
			{
				return BadRequest();
			}
			else
			{
				return Ok();
			}
		}
	}
}