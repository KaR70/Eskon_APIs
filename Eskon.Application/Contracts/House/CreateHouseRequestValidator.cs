namespace Eskon.Application.Contracts.House;

public class CreateHouseRequestValidator : AbstractValidator<CreateHouseRequest>
{
    public CreateHouseRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty();
        
        RuleFor(x => x.PricePerMonth)
            .NotEmpty();

        RuleFor(x => x.NumberOfRooms)
            .NotEmpty();

        RuleFor(x => x.NumberOfBathrooms)
            .NotEmpty();

        RuleFor(x => x.Area)
            .NotEmpty();

        RuleFor(x => x.LocationId)
            .NotEmpty();
        
        RuleFor(x => x.BedCount)
            .GreaterThanOrEqualTo(1)
            .When(x => x.IsShared)
            .WithMessage("Shared housing requires at least one bed.");
    }
}
