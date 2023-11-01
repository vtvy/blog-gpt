namespace BlogGPT.Application.Common.Models
{
    public class PaginatedList<T> where T : class
    {
        public IReadOnlyCollection<T>? Items { get; set; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedList(int pageNumber, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)(pageSize > 0 ? pageSize : 1));
            PageNumber = pageNumber > TotalPages ? TotalPages : pageNumber;
        }
    }
}
