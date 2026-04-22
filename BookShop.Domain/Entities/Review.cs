using BookShop.Domain.Common;

namespace BookShop.Domain.Entities
{
    public class Review : BaseEntity
    {
        public Guid BookId { get; private set; }

        public Guid UserId {get;private set;}

        public User User {get;private set;} = null!;
        public int Rating { get; private set; }
        public string Comment { get; private set; } = string.Empty;

        private Review() { }

        public Review(Guid bookId, Guid UserId, int rating, string comment)
        {

            if (rating < 1 || rating > 5)
                throw new ArgumentException("Invalid rating");

            Id = Guid.NewGuid();
            BookId = bookId;
            Rating = rating;
            Comment = comment;
            CreatedAt = DateTime.UtcNow;
        }

    }
}