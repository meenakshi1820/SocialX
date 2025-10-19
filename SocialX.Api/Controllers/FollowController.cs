using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialX.Api.Data;
namespace SocialX.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FollowController : ControllerBase
{
    private readonly SocialXContext _db;
    public FollowController(SocialXContext db) { _db = db; }
    [HttpPost("{userId:int}")]
    public async Task<IActionResult> FollowUser(int userId)
    {
        var me = User.GetUserId();
        if (me == userId) return BadRequest("Cannot follow yourself.");
        var exists = await _db.Follows.AnyAsync(f => f.FollowerId == me && f.FolloweeId == userId);
        if (exists) return BadRequest("Already following.");
        _db.Follows.Add(new Models.Follow { FollowerId = me, FolloweeId = userId }); await _db.SaveChangesAsync(); return NoContent();
    }
    [HttpDelete("{userId:int}")]
    public async Task<IActionResult> UnfollowUser(int userId)
    {
        var me = User.GetUserId();
        var link = await _db.Follows.FirstOrDefaultAsync(f => f.FollowerId == me && f.FolloweeId == userId);
        if (link is null) return NotFound();
        _db.Follows.Remove(link); await _db.SaveChangesAsync(); return NoContent();
    }
}