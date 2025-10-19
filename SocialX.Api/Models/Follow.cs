namespace SocialX.Api.Models;
public class Follow { 
    public int Id { get; set; } 
    public int FollowerId { get; set; } 
    public User Follower { get; set; } = default!; 
    public int FolloweeId { get; set; } 
    public User Followee { get; set; } = default!; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}