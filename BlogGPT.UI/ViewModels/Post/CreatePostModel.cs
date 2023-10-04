﻿using System.ComponentModel.DataAnnotations;

namespace BlogGPT.UI.ViewModels.Post
{
    public class CreatePostModel
    {
        [Display(Name = "Categories")]
        public int[]? CategoryIDs { get; set; }

        [Required(ErrorMessage = "Post title is required")]
        [Display(Name = "Title")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} from {2} to {1}")]
        public string Title { set; get; } = string.Empty;

        [Display(Name = "Description")]
        public string Description { set; get; } = string.Empty;

        [Display(Name = "Chuỗi định danh (url)", Prompt = "Nhập hoặc để trống tự phát sinh theo Title")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} from {2} to {1}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        public string Slug { set; get; }

        [Display(Name = "Nội dung")]
        public string Content { set; get; }

        [Display(Name = "Xuất bản")]
        public bool Published { set; get; }
    }
}