using BlogGPT.Application.Common.Extensions;
using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Models;

namespace BlogGPT.Application.Categories.Queries
{
    public record GetSelectCategoryQuery : IRequest<IEnumerable<TreeItem<GetSelectCategory>>>
    {
        public int? Id { get; set; }
    };

    public class GetSelectCategoryHandler : IRequestHandler<GetSelectCategoryQuery, IEnumerable<TreeItem<GetSelectCategory>>>
    {
        private readonly IApplicationDbContext _context;

        public GetSelectCategoryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TreeItem<GetSelectCategory>>> Handle(GetSelectCategoryQuery request, CancellationToken cancellationToken)
        {
            var categoriesQuery = _context.Categories.Select(category => new GetSelectCategory
            {
                Id = category.Id,
                Name = category.Name,
                ParentId = category.ParentId,
                Slug = category.Slug
            });

            if (request.Id != null)
            {
                categoriesQuery = categoriesQuery.Where(c => c.Id != request.Id);
            }

            var categories = await categoriesQuery.ToListAsync(cancellationToken);

            var returnCategories = categories.GenerateChildren(c => c.Id, c => c.ParentId);

            return returnCategories;
        }
    }
}