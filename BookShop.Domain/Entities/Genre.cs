using BookShop.Domain.Common;

namespace BookShop.Domain.Entities
{
    public class Genre : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Description { get; private set; }

        public ICollection<Book> Books { get; private set; } = new List<Book>();

        private Genre() { }

        public Genre(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Genre name is required");

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string? description)
        {

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Genre name is required");

            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}