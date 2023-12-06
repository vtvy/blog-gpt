using BlogGPT.Application.Common.Extensions;
using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Domain.Exceptions;

namespace BlogGPT.Application.Categories.Commands
{
    public record UpdateCategoryCommand : IRequest<int>
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }
    }


    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCategoryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories.FindAsync(new object?[] { command.Id }, cancellationToken: cancellationToken) ?? throw new NotFoundException(nameof(Category), command.Id);

            entity.Name = command.Name;
            entity.Description = command.Description;
            entity.ParentId = command.ParentId;
            entity.Slug = command.Name.GenerateSlug();

            var existedSlug = await _context.Categories
                .AnyAsync(cate => cate.Slug == entity.Slug, cancellationToken);

            if (existedSlug)
            {
                var latestCategory = await _context.Categories
                    .OrderByDescending(cate => cate.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (latestCategory != null)
                {
                    entity.Slug = $"{entity.Slug}-{latestCategory.Id + 1}";
                }
                else
                {
                    entity.Slug = $"{entity.Slug}-1";
                }
            };

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}