namespace BlogGPT.Application.Common.Models
{
    public class PaginatedList<T> where T : class
    {
        public IReadOnlyCollection<T>? Items { get; set; }
        public string Filter { get; set; }
        public string Search { get; set; }
        public string Order { get; set; }
        public string Direction { get; set; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedList(string categories, string search, string order, string dir, int pageNumber, int pageSize, int totalCount)
        {
            Filter = categories;
            Search = search;
            Order = order;
            Direction = dir;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)(pageSize > 0 ? pageSize : 1));
            PageNumber = pageNumber > TotalPages ? TotalPages : pageNumber;
        }
    }
}
