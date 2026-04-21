namespace BookShop.Domain.ValueObjects
{
    public class Isbn
    {
        public string Value { get; }

        public Isbn(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("ISBN cannot be empty");

            if (value.Length < 10)
                throw new ArgumentException("Invalid ISBN");

            Value = value;
        }

        public override string ToString() => Value;

    }
}