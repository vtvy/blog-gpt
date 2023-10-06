using BlogGPT.Application.Categories.Queries;

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
            CreateMap<GetAllCategoryVM, SelectCategoryModel>();
        }
    }
}
