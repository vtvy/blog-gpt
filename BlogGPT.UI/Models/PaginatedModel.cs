﻿namespace BlogGPT.UI.Models
{
    public class PaginatedModel<T> where T : class
    {
        public IReadOnlyCollection<T>? Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}