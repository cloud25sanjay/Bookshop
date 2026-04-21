using BookShop.Domain.Common;
using BookShop.Domain.Events;
using BookShop.Domain.ValueObjects;

namespace BookShop.Domain.Entities;

public class Book : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public Guid AuthorId { get; private set; }
    public Author Author { get; private set; } = null!;

    public Isbn ISBN { get; private set; } = null!;

    public Money PhysicalPrice { get; private set; } = null!;
    public Money DigitalPrice { get; private set; } = null!;

    public int StockQuantity { get; private set; }
    public string? DigitalUrl { get; private set; }

    public Guid GenreId { get; private set; }
    public Genre Genre { get; private set; } = null!;

    public double AverageRating { get; private set; }
    public int TotalReviews { get; private set; }

    public ICollection<Review> Reviews { get; private set; } = new List<Review>();

    public bool IsActive { get; private set; } = true;

    private Book() { }

    public static Book Create(
        string title,
        string description,
        Guid authorId,
        Isbn isbn,
        Money physicalPrice,
        Money digitalPrice,
        int stockQuantity,
        string? digitalUrl,
        Guid genreId)
    {
        Validate(title, isbn, physicalPrice, digitalPrice, stockQuantity, digitalUrl);

        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            AuthorId = authorId,
            ISBN = isbn,
            PhysicalPrice = physicalPrice,
            DigitalPrice = digitalPrice,
            StockQuantity = stockQuantity,
            DigitalUrl = digitalUrl,
            GenreId = genreId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        book.AddDomainEvent(new BookCreatedEvent(book.Id));

        return book;
    }

    //  Correct validation
    private static void Validate(
        string title,
        Isbn isbn,
        Money physicalPrice,
        Money digitalPrice,
        int stockQuantity,
        string? digitalUrl)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required");

        if (isbn == null)
            throw new ArgumentException("ISBN is required");

        if (physicalPrice.Amount < 0 || digitalPrice.Amount < 0)
            throw new ArgumentException("Price cannot be negative");

        if (stockQuantity < 0)
            throw new ArgumentException("Stock cannot be negative");

        if (digitalPrice.Amount > 0 && string.IsNullOrWhiteSpace(digitalUrl))
            throw new ArgumentException("Digital book must have URL");
    }

    // Stock
    public void ReduceStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero");

        if (StockQuantity < quantity)
            throw new InvalidOperationException("Not enough stock");

        StockQuantity -= quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero");

        StockQuantity += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    // Review Logic
    public void AddReview(Review review)
    {
        Reviews.Add(review);

        TotalReviews++;

        AverageRating = ((AverageRating * (TotalReviews - 1)) + review.Rating) / TotalReviews;
    }

    // UpdateDetails
    public void UpdateDetails(string title, string description, Money physicalPrice, Money digitalPrice)
    {
        Validate(title, ISBN, physicalPrice, digitalPrice, StockQuantity, DigitalUrl);

        Title = title;
        Description = description;
        PhysicalPrice = physicalPrice;
        DigitalPrice = digitalPrice;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}