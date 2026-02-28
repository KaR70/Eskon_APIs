namespace Eskon.Application.Contracts.Location;

public class LocationRequestValidator : AbstractValidator<LocationRequest>
{
    public LocationRequestValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Street)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.BuildingNumber)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.PostalCode)
            .MaximumLength(20);
    }
}
