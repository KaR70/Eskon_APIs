namespace Eskon.Application.Contracts.House;

public class SetOccupancyTypeRequestValidator : AbstractValidator<SetOccupancyTypeRequest>
{
    public SetOccupancyTypeRequestValidator()
    {
        RuleFor(x => x.BedCount)
            .GreaterThanOrEqualTo(1)
            .When(x => x.isShared)
            .WithMessage("Shared housing requires at least one bed.");
    }
}
