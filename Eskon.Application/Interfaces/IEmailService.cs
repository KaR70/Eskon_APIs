using Eskon.Domain.Entities;

namespace Eskon.Application.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmail(ApplicationUser user, string origin, string code);
    Task SendResetPasswordEmail(ApplicationUser user, string origin, string code);
}