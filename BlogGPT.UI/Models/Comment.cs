using BlogGPT.Application.Common.Models;

namespace BlogGPT.UI.Models
{
    public class Comment
    {
        public required string Author { get; set; }

        public int Id { get; set; }

        public required string Content { get; set; }

        public DateTime LastModifiedAt { get; set; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<GetComment, Comment>();
            }
        }
    }
}
