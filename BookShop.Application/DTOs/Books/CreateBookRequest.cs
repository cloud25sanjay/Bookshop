namespace BookShop.Application.DTOs.Books
{
    public class CreateBookRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Guid AuthorId { get; set; }
        public Guid GenreId { get; set; }

        public string ISBN { get; set; } = string.Empty;

        public decimal PhysicalPrice { get; set; }
        public decimal DigitalPrice { get; set; }

        public int StockQuantity { get; set; }

        public string? DigitalUrl { get; set; }
    }
}