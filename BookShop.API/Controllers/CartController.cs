using System.Security.Claims;
using BookShop.Application.DTOs.Cart;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly BookShopDbContext _context;
        public CartController(BookShopDbContext context)
        {
            _context = context;
        }

        // Get Cart
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == Guid.Parse(userId!));

            if (cart == null)
                return Ok(new { Items = new List<Object>() });

            return Ok(cart);
        }

        // Add to Cart
        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart(userId);
                _context.Carts.Add(cart);
            }

            cart.AddItem(request.BookId, request.Quantity);
            await _context.SaveChangesAsync();

            return Ok("Item Added to Cart");
        }

        // Remove form Cart
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> RemoveItem(Guid bookId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound();

            cart.RemoveItem(bookId);

            await _context.SaveChangesAsync();

            return Ok("Item Removed");
        }


        // Update Quantity
        [HttpPut]
        public async Task<IActionResult> UpdateQuantity(UpdateCartRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if(cart == null)
                return NotFound();
            
            cart.UpdateQuantity(request.BookId, request.Quantity);

            await _context.SaveChangesAsync();

            return Ok("Cart Updated");
        }
    }
}