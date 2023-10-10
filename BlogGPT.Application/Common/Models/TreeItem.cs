namespace BlogGPT.Application.Common.Models
{
    public class TreeItem<T> where T : class
    {
        public required T Item { get; set; }
        public IEnumerable<TreeItem<T>>? Children { get; set; }
    }
}