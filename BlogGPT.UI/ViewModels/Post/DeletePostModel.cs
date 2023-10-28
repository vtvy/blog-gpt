using BlogGPT.Application.Posts.Queries;

namespace BlogGPT.UI.ViewModels.Post
{
    public class DeletePostModel
    {
        public int Id { get; set; }

        public int[]? CategoryIds { get; set; }

        public required string Title { set; get; }

        public string? Description { set; get; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<GetPostVM, DeletePostModel>();
            }
        }
    }
}