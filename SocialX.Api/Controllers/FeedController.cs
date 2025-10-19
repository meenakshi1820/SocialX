using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialX.Api.Data;
using SocialX.Api.DTOs;
namespace SocialX.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FeedController : ControllerBase
{
    private readonly SocialXContext _db;
    public FeedController(SocialXContext db) { _db = db; }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostResponse>>> GetFeed([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var me = User.GetUserId();
        var followingIds = await _db.Follows.Where(f => f.FollowerId == me).Select(f => f.FolloweeId).ToListAsync();
        var posts = await _db.Posts.Where(p => followingIds.Contains(p.UserId) || p.UserId == me)
            .Include(p => p.User).Include(p => p.Likes).Include(p => p.Comments)
            .OrderByDescending(p => p.CreatedAt).Skip((page - 1) * pageSize).Take(pageSize)
            .Select(p => new PostResponse(p.Id, p.Content, p.CreatedAt, p.UserId, p.User.Username, p.Likes.Count, p.Comments.Count)).ToListAsync();
        return posts;
    }
}