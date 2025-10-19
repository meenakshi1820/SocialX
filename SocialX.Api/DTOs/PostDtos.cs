namespace SocialX.Api.DTOs;
public record CreatePostRequest(string Content);
public record PostResponse(int Id, string Content, DateTime CreatedAt, int UserId, string Username, int LikeCount, int CommentCount);