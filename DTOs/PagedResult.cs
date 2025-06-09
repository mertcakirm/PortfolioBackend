namespace Cors.DTO;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }  // 👈 son sayfa numarası
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}