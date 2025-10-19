using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialX.Api.Data;
using SocialX.Api.DTOs;
using SocialX.Api.Models;
using SocialX.Api.Services;
namespace SocialX.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SocialXContext _db;
    private readonly IJwtTokenService _jwt;
    public AuthController(SocialXContext db, IJwtTokenService jwt) { _db = db; _jwt = jwt; }
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest req)
    {
        if (await _db.Users.AnyAsync(u => u.Email == req.Email || u.Username == req.Username)) return BadRequest("Email or Username already exists.");
        var user = new User { Username = req.Username.Trim(), Email = req.Email.Trim().ToLowerInvariant(), PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password) };
        _db.Users.Add(user); await _db.SaveChangesAsync();
        var token = _jwt.CreateToken(user);
        return new AuthResponse(token, user.Username, user.Id);
    }
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest req)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == req.Email.Trim().ToLowerInvariant());
        if (user is null) return Unauthorized("Invalid credentials.");
        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash)) return Unauthorized("Invalid credentials.");
        var token = _jwt.CreateToken(user);
        return new AuthResponse(token, user.Username, user.Id);
    }
}