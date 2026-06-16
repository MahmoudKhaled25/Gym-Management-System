using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Application.Consts;

public static class IQueryableExtensions
{
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> source,string? sortColumn,string? sortDirection) where T : class
    {
        if (string.IsNullOrEmpty(sortColumn))
            return source;

        return sortDirection == "DESC"
            ? source.OrderByDescending(e => EF.Property<object>(e, sortColumn))
            : source.OrderBy(e => EF.Property<object>(e, sortColumn));
    }
    
}

