namespace Eskon.Application.Contracts.Authentication;


public record ConfirmEmailRequest (
    string Email,
    string Code
    );