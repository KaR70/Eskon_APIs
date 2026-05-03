using FluentValidation;

namespace Eskon.Application.Contracts.Review;

public class CreateReviewRequestValidator : AbstractValidator<CreateReviewRequest>
{
    public CreateReviewRequestValidator()
    {
        RuleFor(x => x.Stars)
            .InclusiveBetween(1, 5)
            .WithMessage("Stars must be between 1 and 5.");

        RuleFor(x => x.Comment)
            .MaximumLength(1000);
    }
}
