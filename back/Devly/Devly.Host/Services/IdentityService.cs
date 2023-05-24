using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Devly.Models;
using Microsoft.IdentityModel.Tokens;

namespace Devly.Services;

public sealed class IdentityService : IIdentityService
{
    private const string TokenSecret = "SuperSecretKey1337";
    private static readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(4);

    public Task<string> GenerateToken(TokenRequestDto tokenRequest)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(TokenSecret);

        var claims = new List<Claim>
        {
            new("Email", tokenRequest.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var claimPair in tokenRequest.CustomClaims)
        {
            var claim = new Claim(claimPair.Key, claimPair.Value.ToString()!, ClaimValueTypes.String);
            claims.Add(claim);
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(TokenLifeTime),
            Issuer = "devly.ru",
            Audience = "anything.devly.ru",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var jwt = tokenHandler.WriteToken(token);
        return Task.FromResult(jwt);
    }
}