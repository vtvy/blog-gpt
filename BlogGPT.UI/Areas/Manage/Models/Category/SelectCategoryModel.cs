using BlogGPT.Application.Common.Models;

namespace BlogGPT.UI.Areas.Manage.Models.Category;
public class SelectCategoryModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Slug { get; set; }

    private class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetSelectCategory, SelectCategoryModel>();
            CreateMap<TreeItem<GetSelectCategory>, TreeModel<SelectCategoryModel>>();
        }
    }
}
