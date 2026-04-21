namespace BookShop.Domain.ValueObjects
{
    public class Money
    {
        public decimal Amount {get;}
        public string Currency {get;}

        public Money(decimal amount, string currency = "INR")
        {
            if(amount < 0)
                throw new ArgumentException("Amount cannot be Negative");
            
            Amount = amount;
            Currency = currency;
        }
    }
}