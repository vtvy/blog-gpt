using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogGPT.Application.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand : IRequest<int>
    {
        public int? ParentId { get; set; }

        [Required(ErrorMessage = "Phải có tên danh mục")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [Display(Name = "Url")]
        public string Slug { set; get; } = string.Empty;
    }
}
