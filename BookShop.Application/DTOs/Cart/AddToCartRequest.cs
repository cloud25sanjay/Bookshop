namespace BookShop.Application.DTOs.Cart
{
    public class AddToCartRequest
    {
        public Guid BookId {get;set;}
        public int Quantity {get;set;}
    }
}