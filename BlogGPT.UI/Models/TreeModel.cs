namespace BlogGPT.UI.Models
{
    public class TreeModel<T> where T : class
    {
        public required T Item { get; set; }
        public IEnumerable<TreeModel<T>>? Children { get; set; }
    }
}
