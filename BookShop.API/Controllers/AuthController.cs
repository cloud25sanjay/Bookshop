using Microsoft.AspNetCore.Mvc;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Persistence;
using BookShop.Infrastructure.Auth;
using BookShop.Application.DTOs.Auth;
using Microsoft.EntityFrameworkCore;

namespace BookShop.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly BookShopDbContext _context;
    private readonly PasswordHasher _hasher;
    private readonly JwtTokenService _jwt;

    public AuthController(
        BookShopDbContext context,
        PasswordHasher hasher,
        JwtTokenService jwt)
    {
        _context = context;
        _hasher = hasher;
        _jwt = jwt;
    }

    // 🔐 REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (existingUser != null)
            return BadRequest("User already exists");

        var hashedPassword = _hasher.Hash(request.Password);

        var user = new User(
            request.FullName,
            request.Email,
            hashedPassword
        );

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _jwt.CreateToken(user);

        return Ok(new AuthResponse
        {
            Token = token,
            Email = user.Email,
            Role = user.Role
        });
    }

    // 🔑 LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user == null)
            return Unauthorized("Invalid credentials");

        var isValid = _hasher.Verify(request.Password, user.PasswordHash);

        if (!isValid)
            return Unauthorized("Invalid credentials");

        var token = _jwt.CreateToken(user);

        return Ok(new AuthResponse
        {
            Token = token,
            Email = user.Email,
            Role = user.Role
        });
    }
}