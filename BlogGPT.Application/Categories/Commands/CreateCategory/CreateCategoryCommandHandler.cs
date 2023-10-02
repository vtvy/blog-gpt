using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Application.Common.Services;
using BlogGPT.Domain.Entities;

namespace BlogGPT.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateCategoryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var entity = new Category
            {
                Name = command.Name,
                Slug = Utility.GenerateSlug(command.Name)
            };

            _context.Categories.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
