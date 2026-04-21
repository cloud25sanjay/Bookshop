using BookShop.Domain.Common;

namespace BookShop.Domain.Events
{
    public class BookCreatedEvent : IDomainEvent
    {
        public Guid BookId {get;}
        public DateTime OccurredOn {get;}

        public BookCreatedEvent(Guid bookId)
        {
            BookId = bookId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}