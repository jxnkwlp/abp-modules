using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Passingwind.Abp.FileManagement.EntityFrameworkCore;

public static class FileItemExtensions
{
    public static IQueryable<FileItem> IncludeAll(this IQueryable<FileItem> query)
    {
        return query.Include(x => x.Tags).Include(x => x.Path);
    }

    public static IQueryable<FileItem> IncludeDetails(this IQueryable<FileItem> query, bool include = false)
    {
        if (include) return IncludeAll(query);

        return query;
    }
}
