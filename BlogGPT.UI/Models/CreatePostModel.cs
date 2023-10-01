using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Models
{
    public class CreatePostModel : Post
    {
        [Display(Name = "Categories")]
        public int[] CategoryIDs { get; set; }
    }
}