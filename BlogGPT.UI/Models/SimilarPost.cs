using BlogGPT.Application.Common.Models;

namespace BlogGPT.UI.Models
{
    public class SimilarPost
    {
        public required string Title { get; set; }

        public string? Description { get; set; }

        public required string Slug { get; set; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<GetSimilarPost, SimilarPost>();
            }
        }
    }
}
