using BlogGPT.Application.Categories.Commands.CreateCategory;
using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Models.Category
{
    public class CreateCategoryModel
    {
        [Display(Name = "Parent category")]
        public int? ParentId { get; set; }

        [Display(Name = "Category name")]
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100, ErrorMessage = "{0} must less than {1}")]
        public string Name { get; set; } = string.Empty;

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<CreateCategoryModel, CreateCategoryCommand>();
            }
        }
    }
}
