using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.Models.Category
{
    public class CreateCategoryModel
    {
        public int? ParentId { get; set; }

        [Required(ErrorMessage = "Phải có tên danh mục")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        public string Name { get; set; } = string.Empty;
    }
}
