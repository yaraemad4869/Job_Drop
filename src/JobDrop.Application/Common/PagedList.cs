namespace JobDrop.Application.Common;

public sealed class PagedList<T>
{
    public IReadOnlyList<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;

    public PagedList(IReadOnlyList<T> items, int totalCount, int page, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }

    public static PagedList<T> Empty(int page = 1, int pageSize = 20)
        => new(Array.Empty<T>(), 0, page, pageSize);
}

public sealed class CursorPage<T>
{
    public IReadOnlyList<T> Items { get; }
    public string? NextCursor { get; }
    public bool HasMore { get; }

    public CursorPage(IReadOnlyList<T> items, string? nextCursor, bool hasMore)
    {
        Items = items;
        NextCursor = nextCursor;
        HasMore = hasMore;
    }
}