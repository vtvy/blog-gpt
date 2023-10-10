namespace BlogGPT.Domain.Common
{
    public interface ITree<T>
    {
        public T? Parent { get; set; }
        public ICollection<T>? Children { get; set; }
    }
}