using BlogGPT.Application.Common.Models;

namespace BlogGPT.UI.Areas.Manage.Models.Post
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
                CreateMap<GetPost, DeletePostModel>();
            }
        }
    }
}