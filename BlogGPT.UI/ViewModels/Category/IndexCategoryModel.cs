using BlogGPT.Application.Categories.Queries;
using BlogGPT.Application.Common.Models;

namespace BlogGPT.UI.ViewModels.Category
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
