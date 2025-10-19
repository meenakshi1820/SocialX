using Microsoft.EntityFrameworkCore;
using SocialX.Api.Models;
namespace SocialX.Api.Data;
public class SocialXContext : DbContext
{
    public SocialXContext(DbContextOptions<SocialXContext> options) : base(options) {}
    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Follow> Follows => Set<Follow>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<Follow>().HasOne(f => f.Follower).WithMany(u => u.Following).HasForeignKey(f => f.FollowerId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Follow>().HasOne(f => f.Followee).WithMany(u => u.Followers).HasForeignKey(f => f.FolloweeId).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Like>().HasIndex(l => new { l.PostId, l.UserId }).IsUnique();
        base.OnModelCreating(modelBuilder);
    }
}