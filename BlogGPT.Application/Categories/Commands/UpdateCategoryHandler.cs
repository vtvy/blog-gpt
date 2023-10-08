using BlogGPT.Application.Common.Extensions;
using BlogGPT.Application.Common.Interfaces.Data;
using BlogGPT.Domain.Exceptions;

namespace BlogGPT.Application.Categories.Commands
{
    public record UpdateCategoryCommand : IRequest
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }
    }


    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCategoryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories.FindAsync(new object?[] { command.Id }, cancellationToken: cancellationToken) ?? throw new NotFoundException(nameof(Category), command.Id);

            entity.Name = command.Name;
            entity.Description = command.Description;
            entity.ParentId = command.ParentId;
            entity.Slug = command.Name.GenerateSlug();

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}