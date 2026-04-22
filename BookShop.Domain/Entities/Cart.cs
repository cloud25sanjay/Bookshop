using BookShop.Domain.Common;

namespace BookShop.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public Guid UserId { get; private set; }

        public ICollection<CartItem> Items { get; private set; } = new List<CartItem>();

        private Cart() { }

        public Cart(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }


        // Add Item
        public void AddItem(Guid bookId, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.BookId == bookId);

            if (existingItem != null)
            {
                existingItem.Increase(quantity);
                return;
            }

            Items.Add(new CartItem(bookId, quantity));
        }

        // Remove Item
        public void RemoveItem(Guid bookId)
        {
            var item = Items.FirstOrDefault(i => i.BookId == bookId);

            if (item != null)
                Items.Remove(item);
        }


        //  Update Quantity
        public void UpdateQuantity(Guid bookId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.BookId == bookId);

            if (item == null)
                throw new Exception("Item not found");

            item.Update(quantity);
        }


    }
}