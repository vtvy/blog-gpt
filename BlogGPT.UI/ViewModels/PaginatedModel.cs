namespace BlogGPT.UI.ViewModels
{
    public class PaginatedModel<T> where T : class
    {
        public IReadOnlyCollection<T>? Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public Func<int?, string> GenerateUrl { get; set; } = null!;

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}
