using BlogGPT.UI.Areas.Manage.Models.Category;

namespace BlogGPT.UI.Models
{
    public class PaginatedModel
    {
        public string Order { get; set; } = "date";
        public string Direction { get; set; } = "desc";
        public int PageNumber { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public string PageSize { get; set; } = "10";
        public IList<SelectCategoryModel> CategoryList { get; set; } = new List<SelectCategoryModel>();
        public string Search { get; set; } = "";
        public string[]? Categories { get; set; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public int PageIndex => int.TryParse(PageSize, out int pageSize) ? pageSize * (PageNumber - 1) : 10 * (PageNumber - 1);
    }
}
