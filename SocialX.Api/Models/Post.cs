namespace SocialX.Api.Models;
public class Post {
    public int Id { get; set; } 
    public string Content { get; set; } = default!; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    public int UserId { get; set; } 
    public User User { get; set; } = default!; 
    public ICollection<Comment> Comments { get; set; } = new List<Comment>(); 
    public ICollection<Like> Likes { get; set; } = new List<Like>(); 
}