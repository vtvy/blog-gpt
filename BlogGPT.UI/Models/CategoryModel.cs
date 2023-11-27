using BlogGPT.Application.Common.Models;

namespace BlogGPT.UI.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Slug { set; get; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<GetAllCategory, CategoryModel>();
                CreateMap<TreeItem<GetAllCategory>, TreeModel<CategoryModel>>();
            }
        }
    }
}