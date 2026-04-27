using System.Security.Cryptography;
using System.Text;
using Eskon.Application.Contracts.Authentication;
using Eskon.Application.Interfaces;
using Eskon.Application.Interfaces.Authentication;
using Eskon.Domain.Abstraction;
using Eskon.Domain.Abstraction.Consts;
using Eskon.Domain.Entities;
using Eskon.Domain.Errors;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Eskon.Application.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtProvider jwtProvider,
    ILogger<AuthService> logger,
    IEmailService emailService) : IAuthService
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IEmailService _emailService = emailService;

    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        if (await userManager.FindByEmailAsync(email) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

        if (result.Succeeded)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var (token, expiresIn) = _jwtProvider.GenerateToken(user,userRoles);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await userManager.UpdateAsync(user);
            var isAdmin = userRoles.Contains("Admin");  // 👈 replace with your admin role name


            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration,isAdmin);

            return Result.Success(response);
        }

        return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials);
    }


    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.isActve);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var userRoles = await userManager.GetRolesAsync(user);

        var (newToken, expiresIn) = _jwtProvider.GenerateToken(user, userRoles);
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration
        });
        var isAdmin = userRoles.Contains("Admin");  // 👈 replace with your admin role nam
        await userManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration,isAdmin);

        return Result.Success(response);
    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.isActve);

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await userManager.UpdateAsync(user);

        return Result.Success();
    }

    public async Task<Result> RegisterAsync(RegisterRequest request, string origin, CancellationToken cancellationToken = default)
    {
        var emailIsExists = await userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

        if (emailIsExists)
            return Result.Failure(UserErrors.DuplicatedEmail);

        var user = request.Adapt<ApplicationUser>();
        user.UserName = request.Email;

        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Confirmation code: {code}", code);

            //await SendConfirmationEmail(user, code);
            await emailService.SendConfirmationEmail(user, origin, code);
            
            return Result.Success();
        }

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, ErrorType.BadRequest));
    }

    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        if (await userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure(UserErrors.InvalidCode);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var code = request.Code;

        try
        {
            code = Encoding.UTF8.GetString((byte[])WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var result = await userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded) { 

            await userManager.AddToRoleAsync(user, DefaultRoles.Member);

            return Result.Success();

        }

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, ErrorType.BadRequest));
    }

    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request, string origin)
    {
        if (await userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation("Confirmation code: {code}", code);

        //await SendConfirmationEmail(user, code);
        await emailService.SendConfirmationEmail(user, origin, code);
        return Result.Success();
    }

    public async Task<Result> SendResetPasswordCodeAsync(string email, string origin)
    {
        if (await userManager.FindByEmailAsync(email) is not { } user)
            return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailNotConfirmed);

        var code = await userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation("Reset code: {code}", code);

        //await SendResetPasswordEmail(user, code);
        await emailService.SendResetPasswordEmail(user, origin, code);

        return Result.Success();
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null || !user.EmailConfirmed)
            return Result.Failure(UserErrors.InvalidCode);

        IdentityResult result;

        try
        {
            var code = Encoding.UTF8.GetString((byte[])WebEncoders.Base64UrlDecode(request.Code));
            result = await userManager.ResetPasswordAsync(user, code, request.NewPassword);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken());
        }

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, ErrorType.Unauthorized));
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    // private async Task SendConfirmationEmail(ApplicationUser user, string origin, string code)
    // {
    //     var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
    //     
    //     var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
    //         templateModel: new Dictionary<string, string>
    //         {
    //             { "{{name}}", user.FirstName },
    //             { "{{action_url}}", $"{origin}/auth/emailConfirmation?code={code}" }
    //         }
    //     );
    //     
    //     await _emailSender.SendEmailAsync(user.Email!, "✅ Eskon: Email Confirmation", emailBody);
    //     
    // }
    //
    //
    // private async Task SendResetPasswordEmail(ApplicationUser user, string origin string code)
    // {
    //     var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
    //
    //     var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
    //         templateModel: new Dictionary<string, string>
    //         {
    //             { "{{name}}", user.FirstName },
    //             { "{{action_url}}", $"{origin}/auth/forgetPassword?email={user.Email}&code={code}" }
    //         }
    //     );
    //
    //     await _emailSender.SendEmailAsync(user.Email!, "✅ Eskon: Change Password", emailBody);
    // }
}