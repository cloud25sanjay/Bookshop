namespace BookShop.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }

        public Guid BookId { get; private set; }

        public decimal Price { get; private set; }

        public int Quantity { get; private set; }

        private OrderItem() { }

        public OrderItem(Guid bookId, decimal price, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Invalid quantity");

            Id = Guid.NewGuid();
            BookId = bookId;
            Price = price;
            Quantity = quantity;
        }
    }
}