namespace SocialX.Api.DTOs;
public record CreateCommentRequest(string Text);
public record CommentResponse(int Id, string Text, DateTime CreatedAt, int UserId, string Username);