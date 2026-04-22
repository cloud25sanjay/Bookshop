using System.Security.Claims;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly BookShopDbContext _context;
        public OrderController(BookShopDbContext context)
        {
            _context = context;
        }

        // Checkout
        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
                return BadRequest("Cart Is Empty");

            var order = new Order(userId);

            foreach (var item in cart.Items)
            {
                var book = await _context.Books.FindAsync(item.BookId);

                if (book == null)
                    return BadRequest("Invalid Book");

                if (book.StockQuantity < item.Quantity)
                    return BadRequest("Not Enough stock");

                // Reduce stock
                book.ReduceStock(item.Quantity);

                order.AddItem(item.BookId, book.PhysicalPrice.Amount, item.Quantity);
            }

            order.MarkAsPaid();

            _context.Carts.Remove(cart);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Order placed successfully 🎉",
                OrderId = order.Id,
                Total = order.TotalAmount
            });
        }

        //  GET USER ORDERS
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var orders = await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return Ok(orders);
        }
    }
}