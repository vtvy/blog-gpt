using BlogGPT.UI.ViewModels.Category;
using BlogGPT.UI.ViewModels.Post;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogGPT.UI.ViewModels.PostCategory
{
    [Table("PostCategory")]
    public class PostCategoryModel
    {
        public int PostID { set; get; }

        public int CategoryID { set; get; }

        [ForeignKey("PostID")]
        public PostModel Post { set; get; }

        [ForeignKey("CategoryID")]
        public IndexCategoryModel Category { set; get; }
    }
}