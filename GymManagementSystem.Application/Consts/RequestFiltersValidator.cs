using FluentValidation;

namespace GymManagementSystem.Application.Common;

public class RequestFiltersValidator : AbstractValidator<RequestFilters>
{
    public RequestFiltersValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.SortDirection)
            .Must(x => x == "ASC" || x == "DESC")
            .When(x => x.SortDirection is not null)
            .WithMessage("SortDirection must be ASC or DESC");
    }
}