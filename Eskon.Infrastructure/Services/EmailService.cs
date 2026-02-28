using System.Web;
using Eskon.Application.Interfaces;
using Eskon.Domain.Settings;
using Eskon.Infrastructure.Helpers;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Eskon.Infrastructure.Services;

public class EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger, IHttpContextAccessor httpContextAccessor) : IEmailService, IEmailSender
{
    private readonly MailSettings _mailSettings = mailSettings.Value;
    private readonly ILogger<EmailService> _logger = logger;
    
    public async Task SendConfirmationEmail(ApplicationUser user, string origin, string code)
    {
        // var origin = httpContextAccessor.HttpContext?.Request.Headers.Origin;
        //
        // var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
        //     templateModel: new Dictionary<string, string>
        //     {
        //         { "{{name}}", user.FirstName },
        //         { "{{action_url}}", $"{origin}/auth/emailConfirmation?code={code}" }
        //     }
        // );
        //
        // await SendEmailAsync(user.Email!, "✅ Eskon: Email Confirmation", emailBody);
        
        var confirmationLink = $"{origin}/auth/emailConfirmation?code={HttpUtility.UrlEncode(code)}&email={HttpUtility.UrlEncode(user.Email)}";
            
        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation", new Dictionary<string, string>
        {
            { "{{name}}", user.FirstName },
            { "{{action_url}}", confirmationLink }
        });

        await SendEmailAsync(user.Email!, "✅ Eskon: Email Confirmation", emailBody);
    }

    public async Task SendResetPasswordEmail(ApplicationUser user, string origin, string code)
    {
        // var origin = httpContextAccessor.HttpContext?.Request.Headers.Origin;
        //
        // var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
        //     templateModel: new Dictionary<string, string>
        //     {
        //         { "{{name}}", user.FirstName },
        //         { "{{action_url}}", $"{origin}/auth/forgetPassword?email={user.Email}&code={code}" }
        //     }
        // );
        //
        // await SendEmailAsync(user.Email!, "✅ Eskon: Change Password", emailBody);
        
        var resetLink = $"{origin}/auth/resetPassword?email={HttpUtility.UrlEncode(user.Email)}&code={HttpUtility.UrlEncode(code)}";
            
        var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword", new Dictionary<string, string>
        {
            { "{{name}}", user.FirstName },
            { "{{action_url}}", resetLink }
        });

        await SendEmailAsync(user.Email!, "✅ Eskon: Change Password", emailBody);
    }
    
    
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_mailSettings.Mail),
            Subject = subject
        };

        message.To.Add(MailboxAddress.Parse(email));

        var builder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };

        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        _logger.LogInformation("Sending email to {email}", email);

        try
        {
            await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {email}", email);
            throw;
        }
    }
}