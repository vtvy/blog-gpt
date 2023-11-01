using BlogGPT.Application.Common.Models;

namespace BlogGPT.UI.ViewModels.Category
{
    public class DetailCategoryModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Slug { get; set; }

        public string? Description { get; set; }

        public int? ParentId { set; get; }

        public string? Parent { set; get; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<GetCategory, DetailCategoryModel>();
            }
        }
    }
}
