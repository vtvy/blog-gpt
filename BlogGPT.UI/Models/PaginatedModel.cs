namespace BlogGPT.UI.Models
{
    public class PaginatedModel
    {
        public string Filter { get; set; } = "";
        public string Order { get; set; } = "date";
        public string Direction { get; set; } = "desc";
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public int PageIndex => (PageNumber - 1) * PageSize;
    }
}
