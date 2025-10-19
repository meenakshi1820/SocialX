using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialX.Api.Models;
namespace SocialX.Api.Services;
public class JwtSettings { 
    public string Issuer { get; set; } = default!; 
    public string Audience { get; set; } = default!; 
    public string Key { get; set; } = default!; 
    public int ExpiresMinutes { get; set; }
}
public interface IJwtTokenService { string CreateToken(User user);
}
public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _settings;
    public JwtTokenService(IOptions<JwtSettings> settings) { _settings = settings.Value; }
    public string CreateToken(User user)
    {
        var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), new Claim(JwtRegisteredClaimNames.UniqueName, user.Username), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: _settings.Issuer, audience: _settings.Audience, claims: claims, expires: DateTime.UtcNow.AddMinutes(_settings.ExpiresMinutes), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}