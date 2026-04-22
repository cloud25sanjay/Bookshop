namespace BookShop.Application.Books.DTOs;

public class BookQueryParams
{
    public string? Search { get; set; }
    public Guid? GenreId { get; set; }
    public Guid? AuthorId { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}