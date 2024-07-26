using FluentValidation;
using TournamentSimulator.Core.Entities;

namespace TournamentSimulator.Core.Validators;

public class TeamValidator : AbstractValidator<Team>
{
    public TeamValidator()
    {
        RuleFor(team => team.Name).NotEmpty().MaximumLength(50);
        RuleFor(team => team.Strength).InclusiveBetween(1, 100);
    }
}