using BlogGPT.Application.Common.Models;
using BlogGPT.UI.Models;

namespace BlogGPT.UI.Areas.Manage.Models.Category;
public class SelectCategoryModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    private class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetSelectCategory, SelectCategoryModel>();
            CreateMap<TreeItem<GetSelectCategory>, TreeModel<SelectCategoryModel>>();
        }
    }
}
