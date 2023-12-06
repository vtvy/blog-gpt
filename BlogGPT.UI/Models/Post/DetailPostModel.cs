using BlogGPT.Application.Common.Models;

namespace BlogGPT.UI.Models.Post
{
    public class DetailPostModel
    {
        public int Id { get; set; }

        public IEnumerable<CategoryModel>? Categories { get; set; }

        public required string Title { set; get; }

        public string? Description { set; get; }

        public required string Content { set; get; }

        public bool IsPublished { set; get; }

        public string? Thumbnail { get; set; }

        public required string Slug { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public string? LastModifiedBy { get; set; }

        public string? CreatedBy { get; set; }

        public string? AddComment { get; set; }
        public int AddCommentId { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<GetDetailPost, DetailPostModel>();
            }
        }
    }
}