namespace Eskon.Application.Contracts.House;

public class UpdateHouseRequestValidator : AbstractValidator<UpdateHouseRequest>
{
    public UpdateHouseRequestValidator()
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
    }
}
