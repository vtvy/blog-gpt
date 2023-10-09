using BlogGPT.Application.Categories.Queries;
using BlogGPT.Application.Common.Models;

namespace BlogGPT.UI.ViewModels.Category;
public class SelectCategoryModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public ICollection<SelectCategoryModel>? ChildrenCategories { get; set; }

    private class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetSelectCategoryVM, SelectCategoryModel>();
            CreateMap<TreeItem<GetSelectCategoryVM>, TreeItem<SelectCategoryModel>>();
        }
    }
}
