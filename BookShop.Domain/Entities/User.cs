using BookShop.Domain.Common;

namespace BookShop.Domain.Entities;

public class User : BaseEntity
{
    public string FullName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string Role { get; private set; } = "User"; // User / Admin

    private User() { }

    public User(string fullName, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Name required");

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email required");

        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    public void MakeAdmin()
    {
        Role = "Admin";
        UpdatedAt = DateTime.UtcNow;
    }
}