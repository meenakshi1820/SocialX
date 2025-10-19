namespace SocialX.Api.Models;
public class Comment { 
    public int Id { get; set; } 
    public string Text { get; set; } = default!; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public int PostId { get; set; } 
    public Post Post { get; set; } = default!; 
    public int UserId { get; set; } 
    public User User { get; set; } = default!; 
}