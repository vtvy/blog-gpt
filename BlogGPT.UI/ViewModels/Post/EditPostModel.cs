using BlogGPT.Application.Posts.Commands;
using BlogGPT.Application.Posts.Queries;
using BlogGPT.UI.Constants;
using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.ViewModels.Post
{
    public class EditPostModel
    {
        public int Id { get; set; }

        [Display(Name = "Categories")]
        public int[]? CategoryIds { get; set; }

        [Required(ErrorMessage = "Post title is required")]
        [StringLength(Lengths.Medium, ErrorMessage = "{0} less than {1} characters")]
        public required string Title { set; get; }

        public string? Description { set; get; }

        public required string Content { set; get; }

        public bool IsPublished { set; get; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<EditPostModel, UpdatePostCommand>();
                CreateMap<GetPostVM, EditPostModel>();
            }
        }
    }
}