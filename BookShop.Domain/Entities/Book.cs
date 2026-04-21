namespace BookShop.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; private set; }

        // Book Data
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Author { get; private set; } = string.Empty;
        public string ISBN { get; private set; } = string.Empty;

        public decimal PhysicalPrice { get; private set; }
        public decimal DigitalPrice { get; private set; }

        public int StockQuantity { get; private set; }    //Stock

        public string? DigitalUrl { get; private set; }    //Digital File

        public Guid GenreId { get; private set; }
        public Genre Genre { get; private set; } = null!;

        // Ratings
        public double AverageRating { get; private set; }
        public int TotalReviews { get; private set; }

        // Navigation
        public ICollection<Review> Reviews { get; private set; } = new List<Review>();

        public bool IsActive { get; private set; } = true;  //Status Controll

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }


        // EF core
        private Book() { }


        // Factory Method
        public static Book Create(string title, string description, string author, string isbn, decimal physicalPrice, decimal digitalPrice, int stockQuantity, string? digitalUrl, Guid genreId)
        {

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is required");

            if (physicalPrice < 0 || digitalPrice < 0)
                throw new ArgumentException("Price cannot be negative");

            if (stockQuantity < 0)
                throw new ArgumentException("Stock cannot be negative");

            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("ISBN is required");

            return new Book
            {
                Id = Guid.NewGuid(),

                Title = title,
                Description = description,
                Author = author,
                ISBN = isbn,
                PhysicalPrice = physicalPrice,
                DigitalPrice = digitalPrice,
                StockQuantity = stockQuantity,
                DigitalUrl = digitalUrl,
                GenreId = genreId,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
        }


        // Stock Logic
        public void ReduceStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            if (StockQuantity < quantity)
                throw new Exception("Not Enough Stock");

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


        // Rating Logic
        public void AddReview(int rating)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5");

            TotalReviews++;

            AverageRating = ((AverageRating * (TotalReviews - 1)) + rating) / TotalReviews;
        }

        public void UpdateDetails(string title, string description, decimal physicalPrice, decimal digitalPrice)
        {
            Title = title;
            Description = description;
            PhysicalPrice = physicalPrice;
            DigitalPrice = digitalPrice;

            UpdatedAt = DateTime.UtcNow;
        }


        // Admin Controls
        public void Deactivate()
        {
            IsActive = false;
            
            UpdatedAt = DateTime.UtcNow;
        }
    }
}