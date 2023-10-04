using BlogGPT.Application.Categories.Queries.GetCategory;
using BlogGPT.UI.ViewModels.PostCategory;

namespace BlogGPT.UI.ViewModels.Category
{
    public class DetailCategoryModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public required string Name { get; set; }

        public required string Slug { set; get; }

        public ICollection<DetailCategoryModel>? ChildrenCategories { get; set; }

        public DetailCategoryModel? Parent { set; get; }

        public List<PostCategoryModel>? PostCategories { get; set; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<GetCategoryVM, DetailCategoryModel>();
            }
        }
    }
}
