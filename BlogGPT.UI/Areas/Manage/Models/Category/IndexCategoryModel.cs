using BlogGPT.Application.Common.Models;
using BlogGPT.UI.Models;

namespace BlogGPT.UI.Areas.Manage.Models.Category
{
    public class IndexCategoryModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Slug { set; get; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<GetAllCategory, IndexCategoryModel>();
                CreateMap<TreeItem<GetAllCategory>, TreeModel<IndexCategoryModel>>();
            }
        }
    }

}
