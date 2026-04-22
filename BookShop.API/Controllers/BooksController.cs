using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BookShop.Infrastructure.Persistence;
using BookShop.Domain.Entities;
using BookShop.Domain.ValueObjects;
using BookShop.Application.DTOs.Books;
namespace BookShop.API.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly BookShopDbContext _context;

    public BooksController(BookShopDbContext context)
    {
        _context = context;
    }

    //   CREATE BOOK (Admin Only)
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateBook(CreateBookRequest request)
    {
        //  Validate Author 
        var authorExists = await _context.Authors
            .AnyAsync(a => a.Id == request.AuthorId);

        if (!authorExists)
            return BadRequest("Invalid AuthorId");

        //  Validate Genre
        var genreExists = await _context.Genres
            .AnyAsync(g => g.Id == request.GenreId);

        if (!genreExists)
            return BadRequest("Invalid GenreId");

        // Create Value Objects
        var isbn = new Isbn(request.ISBN);

        var physicalPrice = new Money(request.PhysicalPrice);
        var digitalPrice = new Money(request.DigitalPrice);

        //  Create Book (DDD)
        var book = Book.Create(
            request.Title,
            request.Description,
            request.AuthorId,
            isbn,
            physicalPrice,
            digitalPrice,
            request.StockQuantity,
            request.DigitalUrl,
            request.GenreId
        );

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Book created successfully",
            BookId = book.Id
        });
    }
}