using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookShop.Domain.Entities;
using BookShop.Domain.ValueObjects;

namespace BookShop.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(200);

        //  ISBN → Value Converter
        builder.Property(b => b.ISBN)
            .HasConversion(
                isbn => isbn.Value,             // to DB
                value => new Isbn(value)        // from DB
            )
            .IsRequired();

        // Money  Owned Type (PhysicalPrice)
        builder.OwnsOne(b => b.PhysicalPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("PhysicalPrice");

            money.Property(m => m.Currency)
                .HasColumnName("PhysicalCurrency");
        });

        //  Money  Owned Type (DigitalPrice)
        builder.OwnsOne(b => b.DigitalPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("DigitalPrice");

            money.Property(m => m.Currency)
                .HasColumnName("DigitalCurrency");
        });

        // Relationships
        builder.HasOne(b => b.Author)
            .WithMany()
            .HasForeignKey(b => b.AuthorId);

        builder.HasOne(b => b.Genre)
            .WithMany(g => g.Books)
            .HasForeignKey(b => b.GenreId);
    }
}