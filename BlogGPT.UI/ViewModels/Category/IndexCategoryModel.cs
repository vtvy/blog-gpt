using BlogGPT.Application.Categories.Queries.GetAllCategory;
using BlogGPT.UI.ViewModels.PostCategory;

namespace BlogGPT.UI.ViewModels.Category
{
    public class IndexCategoryModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public required string Name { get; set; }

        public required string Slug { set; get; }

        public ICollection<IndexCategoryModel>? ChildrenCategories { get; set; }

        public IndexCategoryModel? Parent { set; get; }

        public List<PostCategoryModel>? PostCategories { get; set; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<GetAllCategoryVM, IndexCategoryModel>();
            }
        }
    }

}
