namespace ChaCha.Core.Repositories.Pagination;

public class PageInformation
{
    public int Page { get; private set; }
    public int PageSize { get; private set; }
    public int TotalItems { get; private set; }
    public int TotalPages { get; private set; }
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    public PageInformation(int page, int pageSize, int totalItems)
    {
        Page = page;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
    }
}