using Eskon.Domain.Entities;

namespace Eskon.Application.Interfaces.Authentication;

public interface IJwtProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user,IEnumerable<string> roles);
    string? ValidateToken(string token);
}
