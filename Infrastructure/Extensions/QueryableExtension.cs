using Application.ValueObjects.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions;
internal static class QueryableExtension
{
    internal static async Task<PaginationResponse<T>> ToPaginatedResponseAsync<T>(
       this IQueryable<T> query,
       int pageNumber,
       int pageSize)
    {
        var paginatedResult = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize + 1)
            .ToListAsync();

        return new PaginationResponse<T>
        {
            PageNumber = pageNumber,
            HasNext = paginatedResult.Count > pageSize,
            Data = paginatedResult.Take(pageSize).ToList()
        };
    }
}
