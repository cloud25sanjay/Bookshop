using BookShop.Domain.Common;

namespace BookShop.Domain.Entities
{
    public class Author : BaseEntity
    {
        public string Name {get; private set;} = string.Empty;

        public string Bio {get;private set;} = string.Empty;

        public Author() { }

        public Author(string name, string bio)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Author Name Required");

            Id = Guid.NewGuid();
            Name = name;
            Bio = bio;
            CreatedAt = DateTime.UtcNow;
        }
    }
}