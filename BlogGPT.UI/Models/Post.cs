using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogGPT.UI.Models
{
    [Table("Post")]
    public class Post
    {
        [Key]
        public int PostId { set; get; }

        [Required(ErrorMessage = "Phải có tiêu đề bài viết")]
        [Display(Name = "Tiêu đề")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} from {1} to {2}")]
        public string Title { set; get; }

        [Display(Name = "Mô tả ngắn")]
        public string Description { set; get; }

        [Display(Name = "Chuỗi định danh (url)", Prompt = "Nhập hoặc để trống tự phát sinh theo Title")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} from {1} to {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        public string Slug { set; get; }

        [Display(Name = "Nội dung")]
        public string Content { set; get; }

        [Display(Name = "Xuất bản")]
        public bool Published { set; get; }

        public List<PostCategory> PostCategories { get; set; }

        [Display(Name = "Tác giả")]
        public string AuthorId { set; get; }
        //[ForeignKey("AuthorId")]
        //[Display(Name = "Tác giả")]
        //public BlogUser Author { set; get; }



        [Display(Name = "Ngày tạo")]
        public DateTime CreatedAt { set; get; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime LastModifiedAt { set; get; }
    }
}