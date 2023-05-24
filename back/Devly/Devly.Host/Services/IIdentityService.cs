using Devly.Models;

namespace Devly.Services;

public interface IIdentityService
{
    public Task<string> GenerateToken(TokenRequestDto tokenRequest);
}