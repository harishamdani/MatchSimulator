using FluentValidation;
using TournamentSimulator.Core.Entities;

namespace TournamentSimulator.Core.Validators;

public class GroupValidator : AbstractValidator<Group>
{
    public GroupValidator()
    {
        RuleFor(group => group.Teams).NotEmpty().WithMessage("A group must have at least one team.");
        RuleForEach(group => group.Teams).SetValidator(new TeamValidator());
        RuleFor(group => group.Matches).NotNull();
    }
}