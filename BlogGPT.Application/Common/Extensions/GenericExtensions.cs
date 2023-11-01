using BlogGPT.Application.Common.Models;

namespace BlogGPT.Application.Common.Extensions
{
    public static class GenericExtensions
    {
        public static IEnumerable<TreeItem<T>> GenerateChildren<T, K>(
            this IEnumerable<T> source,
            Func<T, K> idSelector,
            Func<T, K> parentIdSelector,
            K? rootId = default) where T : class
        {
            foreach (var treeItem in source.Where(item => EqualityComparer<K>.Default.Equals(parentIdSelector(item), rootId)))
            {
                yield return new TreeItem<T>
                {
                    Item = treeItem,
                    Children = source.GenerateChildren(idSelector, parentIdSelector, idSelector(treeItem))
                };

            }
        }
    }
}
