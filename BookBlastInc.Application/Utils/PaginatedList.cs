namespace BookBlastInc.Application.Utils;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = Convert.ToInt32(Math.Ceiling(count/ (double) pageSize));

        AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public static Func<IEnumerable<T>, int, int, PaginatedList<T>> CreateList = (source, pageIndex, pageSize) =>
    {
        var count = source.Count();
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    };
}