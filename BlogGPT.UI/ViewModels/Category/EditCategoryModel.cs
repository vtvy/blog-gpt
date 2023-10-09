using BlogGPT.Application.Categories.Commands;
using BlogGPT.Application.Categories.Queries;
using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.ViewModels.Category
{
    public class EditCategoryModel
    {
        public int Id { get; set; }

        [Display(Name = "Parent category")]
        public int? ParentId { get; set; }

        [Display(Name = "Category name")]
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100, ErrorMessage = "{0} must less than {1}")]
        public required string Name { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<GetCategoryVM, EditCategoryModel>();
                CreateMap<EditCategoryModel, UpdateCategoryCommand>();
            }
        }

    }

}
