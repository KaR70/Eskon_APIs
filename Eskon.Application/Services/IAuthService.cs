using Eskon.Application.Contracts.Authentication;
using Eskon.Domain.Abstraction;

namespace Eskon.Application.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RegisterAsync(RegisterRequest request, string origin, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request, string origin);
    Task<Result> SendResetPasswordCodeAsync(string email, string origin);
    Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
}
