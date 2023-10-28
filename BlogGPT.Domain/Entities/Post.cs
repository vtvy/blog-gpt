﻿namespace BlogGPT.Domain.Entities
{
    public class Post : BaseEntity
    {
        public required string Title { get; set; }

        public string? Description { get; set; }

        public string Slug { get; set; } = null!;

        public Image? Thumbnail { get; set; }

        public required string Content { get; set; }

        public bool IsPublished { get; set; }

        public ICollection<PostCategory>? PostCategories { get; set; }

        public ICollection<Comment>? Comments { get; set; }

        public EmbeddingPost? EmbeddingPost { get; set; }
    }
}
