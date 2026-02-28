namespace Eskon.Domain.Abstraction;

public record Error(string Code , string Description, ErrorType Type)
{
    public static readonly Error None = new(string.Empty, string.Empty,ErrorType.Failure);
}

public enum ErrorType { Failure, Validation, NotFound, Conflict, Unauthorized, InternalServerError, Forbidden, BadRequest }

