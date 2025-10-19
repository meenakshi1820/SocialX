namespace SocialX.Api.Models;
public class User {
    public int Id { get; set; } 
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Follow> Followers { get; set; } = new List<Follow>();
    public ICollection<Follow> Following { get; set; } = new List<Follow>(); 
    public ICollection<Like> Likes { get; set; } = new List<Like>(); 
    public ICollection<Comment> Comments { get; set; } = new List<Comment>(); 
}