using BookShop.Domain.Common;

namespace BookShop.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId {get;private set;}

        public decimal TotalAmount {get; private set;}

        public string Status {get; private set;} = "Pending";

        public ICollection<OrderItem> Items {get;private set;} = new List<OrderItem>();

        private Order() { }

        public Order(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        public void AddItem(Guid bookId, decimal price, int quantity)
        {
            Items.Add(new OrderItem(bookId,price,quantity));

            TotalAmount += price * quantity;
        }

        public void MarkAsPaid()
        {
            Status = "Paid";
            UpdatedAt = DateTime.UtcNow;
        }
    }
}