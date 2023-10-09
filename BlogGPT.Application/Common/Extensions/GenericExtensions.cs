using BlogGPT.Application.Common.Models;

namespace BlogGPT.Application.Common.Extensions
{
    public static class GenericExtensions
    {
        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        {
            return queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();
        }

        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        {
            return PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
        }

        public static IEnumerable<TreeItem<T>> GenerateChildren<T, K>(
            this IEnumerable<T> source,
            Func<T, K> idSelector,
            Func<T, K> parentIdSelector,
            K? rootId = default) where T : class
        {
            return source.Where(item => EqualityComparer<K>.Default.Equals(parentIdSelector(item), rootId)).Select(item => new TreeItem<T>
            {
                Item = item,
                Children = source.GenerateChildren(idSelector, parentIdSelector, idSelector(item))
            }).AsEnumerable();
        }
    }
}
