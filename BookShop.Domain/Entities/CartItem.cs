namespace BookShop.Domain.Entities
{

    public class CartItem
    {
        public Guid Id { get; private set; }

        public Guid BookId { get; private set; }

        public int Quantity { get; private set; }

        private CartItem() { }

        public CartItem(Guid bookId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be > 0");

            Id = Guid.NewGuid();
            BookId = bookId;
            Quantity = quantity;
        }

        public void Increase(int qty)
        {
            Quantity += qty;
        }

        public void Update(int qty)
        {
            if (qty <= 0)
                throw new ArgumentException("Invalid quantity");

            Quantity = qty;
        }
    }
}