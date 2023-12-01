using BlogGPT.Application.Common.Models;
using BlogGPT.Application.Posts.Commands;
using BlogGPT.UI.Constants;
using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Areas.Manage.Models.Post
{
    public class EditPostModel
    {
        public int Id { get; set; }

        [Display(Name = "Categories")]
        public int[]? CategoryIds { get; set; }

        [Required(ErrorMessage = "Post title is required")]
        [StringLength(Lengths.XL, ErrorMessage = "{0} less than {1} characters")]
        public required string Title { set; get; }

        public string? Thumbnail { set; get; }

        [StringLength(Lengths.XXXL, ErrorMessage = "{0} less than {1} characters")]
        public string? Description { set; get; }

        public required string Content { set; get; }

        public required string RawText { set; get; }

        public bool IsPublished { set; get; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EditPostModel, UpdatePostCommand>()
                    .ForMember(destination => destination.RawText,
                                opt => opt.MapFrom(src => src.RawText.Replace("\r\n", "\n")));
                CreateMap<GetPost, EditPostModel>();
            }
        }
    }
}