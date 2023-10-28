using BlogGPT.Application.Posts.Commands;
using BlogGPT.UI.Constants;
using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.ViewModels.Post
{
    public class CreatePostModel
    {
        [Display(Name = "Categories")]
        public int[]? CategoryIds { get; set; }

        [Required(ErrorMessage = "Post title is required")]
        [StringLength(Lengths.XL, ErrorMessage = "{0} less than {1} characters")]
        public required string Title { set; get; }

        public string? Description { set; get; }

        public required string Content { set; get; }

        public required string RawText { set; get; }

        public bool IsPublished { set; get; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<CreatePostModel, CreatePostCommand>()
                    .ForMember(destination => destination.RawText,
                                opt => opt.MapFrom(src => src.RawText.Replace("\r\n", "\n")));
            }
        }
    }
}