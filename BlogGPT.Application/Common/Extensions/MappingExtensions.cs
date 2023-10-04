using BlogGPT.Application.Common.Models;

namespace BlogGPT.Application.Common.Extensions
{
    public class MappingExtensions
    {
        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
        {
            return queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();
        }

        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
        {
            return PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
        }
    }
}
