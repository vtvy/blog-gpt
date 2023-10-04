using BlogGPT.Application.Categories.Commands.CreateCategory;
using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.ViewModels.Category
{
    public class CreateCategoryModel
    {
        [Display(Name = "Parent category")]
        public int? ParentId { get; set; }

        [Display(Name = "Category name")]
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100, ErrorMessage = "{0} must less than {1}")]
        public required string Name { get; set; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<CreateCategoryModel, CreateCategoryCommand>();
            }
        }
    }
}
