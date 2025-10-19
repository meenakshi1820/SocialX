using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialX.Api.Data;
using SocialX.Api.DTOs;
using SocialX.Api.Models;
namespace SocialX.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly SocialXContext _db;
    public PostsController(SocialXContext db) { _db = db; }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PostResponse>> Create(CreatePostRequest req)
    {
        var userId = User.GetUserId();
        var post = new Post { Content = req.Content, UserId = userId };
        _db.Posts.Add(post); await _db.SaveChangesAsync();
        var user = await _db.Users.FindAsync(userId);
        var resp = new PostResponse(post.Id, post.Content, post.CreatedAt, userId, user!.Username, 0, 0);
        return CreatedAtAction(nameof(GetById), new { id = post.Id }, resp);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostResponse>> GetById(int id)
    {
        var post = await _db.Posts.Include(p => p.User).Include(p => p.Likes).Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);
        if (post is null) return NotFound();
        return new PostResponse(post.Id, post.Content, post.CreatedAt, post.UserId, post.User.Username, post.Likes.Count, post.Comments.Count);
    }
    [Authorize]
    [HttpPost("{id:int}/like")]
    public async Task<IActionResult> Like(int id)
    {
        var userId = User.GetUserId();
        var exists = await _db.Likes.AnyAsync(l => l.PostId == id && l.UserId == userId);
        if (exists) return BadRequest("Already liked.");
        _db.Likes.Add(new Like { PostId = id, UserId = userId }); await _db.SaveChangesAsync(); return NoContent();
    }
    [Authorize]
    [HttpDelete("{id:int}/like")]
    public async Task<IActionResult> Unlike(int id)
    {
        var userId = User.GetUserId();
        var like = await _db.Likes.FirstOrDefaultAsync(l => l.PostId == id && l.UserId == userId);
        if (like is null) return NotFound();
        _db.Likes.Remove(like); await _db.SaveChangesAsync(); return NoContent();
    }
    [Authorize]
    [HttpPost("{id:int}/comments")]
    public async Task<ActionResult<CommentResponse>> Comment(int id, CreateCommentRequest req)
    {
        var userId = User.GetUserId();
        var c = new Comment { PostId = id, UserId = userId, Text = req.Text };
        _db.Comments.Add(c); await _db.SaveChangesAsync();
        var user = await _db.Users.FindAsync(userId);
        return new CommentResponse(c.Id, c.Text, c.CreatedAt, userId, user!.Username);
    }
}