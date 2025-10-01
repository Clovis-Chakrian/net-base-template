using System.Text.Json.Serialization;

namespace ChaCha.Core.Repositories.Pagination;

public class Page<TEntity> where TEntity : class
{
    public PageInformation PageInformation { get; set; }
    public IEnumerable<TEntity> Items { get; set; }

    public Page()
    {
        
    } 
    public Page(IEnumerable<TEntity> items, int page, int pageSize, int totalItems)
    {
        Items = items;
        PageInformation = new PageInformation(
            page: page,
            pageSize: pageSize,
            totalItems: totalItems
            );
    }
    
    public Page(IEnumerable<TEntity> items, PageInformation pageInformation)
    {
        Items = items;
        PageInformation = new PageInformation(
            page: pageInformation.Page, 
            pageSize: pageInformation.PageSize, 
            totalItems: pageInformation.TotalItems
            );
    }
}