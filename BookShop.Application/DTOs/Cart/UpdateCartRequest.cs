namespace BookShop.Application.DTOs.Cart
{
    public class UpdateCartRequest
    {
        public Guid BookId {get;set;}
        public int Quantity {get;set;}
    }
}