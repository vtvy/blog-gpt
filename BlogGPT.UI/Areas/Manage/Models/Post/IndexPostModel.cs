using BlogGPT.Application.Common.Models;
using BlogGPT.UI.Areas.Manage.Models.Category;

namespace BlogGPT.UI.Areas.Manage.Models.Post
{
    public class IndexPostModel
    {
        public int Id { get; set; }

        public IEnumerable<IndexCategoryModel>? Categories { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public bool IsPublished { get; set; }

        public string? Thumbnail { get; set; }

        public required string Slug { get; set; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<GetAllPost, IndexPostModel>();
            }
        }
    }
}