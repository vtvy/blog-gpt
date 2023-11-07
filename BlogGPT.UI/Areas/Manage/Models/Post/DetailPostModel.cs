using BlogGPT.Application.Common.Models;
using BlogGPT.UI.Areas.Manage.Models.Category;

namespace BlogGPT.UI.Areas.Manage.Models.Post
{
    public class DetailPostModel
    {
        public int Id { get; set; }

        public IEnumerable<IndexCategoryModel>? Categories { get; set; }

        public required string Title { set; get; }

        public string? Description { set; get; }

        public required string Content { set; get; }

        public bool IsPublished { set; get; }

        public string? Thumbnail { get; set; }

        public required string Slug { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public string? LastModifiedBy { get; set; }

        public string? CreatedBy { get; set; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<GetDetailPost, DetailPostModel>();
            }
        }
    }
}