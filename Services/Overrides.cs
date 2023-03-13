using Microsoft.AspNetCore.Mvc.RazorPages;

namespace server.Services
{
    public static class Overrides
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int page, int take)
        {
            return source.Skip((page-1)*take).Take(take);
        }
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int page, int take)
        {
            return source.Skip((page - 1) * take).Take(take);
        }
    }
}
