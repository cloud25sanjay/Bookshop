using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BookShop.Infrastructure.Persistence;
using BookShop.Domain.Entities;
using BookShop.Domain.ValueObjects;
using BookShop.Application.DTOs.Books;
using BookShop.Application.Books.DTOs;
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

    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] BookQueryParams query)
    {
        var booksQuery = _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .AsQueryable();

        // SEARCH (Title / Author)
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();

            booksQuery = booksQuery.Where(b =>
                b.Title.ToLower().Contains(search) ||
                b.Author.Name.ToLower().Contains(search)
            );
        }

        //  FILTER BY GENRE
        if (query.GenreId.HasValue)
        {
            booksQuery = booksQuery.Where(b => b.GenreId == query.GenreId);
        }

        //  FILTER BY AUTHOR
        if (query.AuthorId.HasValue)
        {
            booksQuery = booksQuery.Where(b => b.AuthorId == query.AuthorId);
        }

        //  PAGINATION
        var totalCount = await booksQuery.CountAsync();

        var books = await booksQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(b => new BookResponse
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author.Name,
                Genre = b.Genre.Name,
                PhysicalPrice = b.PhysicalPrice.Amount,
                DigitalPrice = b.DigitalPrice.Amount,
                Rating = b.AverageRating
            })
            .ToListAsync();

        return Ok(new
        {
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            Data = books
        });
    }
}