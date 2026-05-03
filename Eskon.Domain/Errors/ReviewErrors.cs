using Eskon.Domain.Abstraction;

namespace Eskon.Domain.Errors;

public static class ReviewErrors
{
    public static readonly Error ReviewNotFound =
        new("Review.NotFound", "Review not found.", ErrorType.NotFound);

    public static readonly Error AlreadyReviewed =
        new("Review.AlreadyReviewed", "You have already reviewed this house.", ErrorType.Conflict);

    public static readonly Error NotOwner =
        new("Review.NotOwner", "You are not authorized to modify this review.", ErrorType.Forbidden);

    public static readonly Error CannotReviewOwnHouse =
        new("Review.CannotReviewOwnHouse", "You cannot review your own house.", ErrorType.Conflict);
}
