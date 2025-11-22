namespace Eskon_APIs.Contracts.Amenity;

public class AmenityRequestValidator : AbstractValidator<AmenityRequest>
{
    public AmenityRequestValidator()
    {
        RuleFor(x => x.AmenityName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Category)
            .MaximumLength(50);
    }
}
