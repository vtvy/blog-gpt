using System.ComponentModel.DataAnnotations;

namespace BlogGPT.Application.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand : IRequest<int>
    {
        public int? ParentId { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} from {1} to {2}")]
        public string Name { get; set; } = string.Empty;
    }
}
