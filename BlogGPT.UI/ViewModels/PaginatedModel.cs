namespace BlogGPT.UI.ViewModels
{
    public class PaginatedModel
    {
        public int PageNumber { get; set; }

        public int TotalPages { get; set; }

        public Func<int?, string> GenerateUrl { get; set; } = null!;
    }
}
