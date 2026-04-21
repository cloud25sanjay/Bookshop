using Microsoft.EntityFrameworkCore;
using BookShop.Domain.Entities;

namespace BookShop.Infrastructure.Persistence;

public class BookShopDbContext : DbContext
{
    public BookShopDbContext(DbContextOptions<BookShopDbContext> options)
        : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookShopDbContext).Assembly);
    }
}